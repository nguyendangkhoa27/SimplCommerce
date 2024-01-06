using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using NDK.SimplCommerce.Infrastructure;
using NDK.SimplCommerce.Infrastructure.Modules;

namespace NDK.SimplCommerce.Webhost.Extensions;

public static class IServiceCollectionExtensions
{
    private static readonly ModuleConfigManager _moduleConfigManager = new ModuleConfigManager();

    public static IServiceCollection AddModules(this IServiceCollection services)
    {
        var modules = _moduleConfigManager.GetModules();
        foreach(var module in modules){
            if(!module.IsBundledWithHost){
                TryLoadAssemblyModule(module.Id,module);
            }else{
                module.Assembly = Assembly.Load(new AssemblyName(module.Id));
            }
            if(module.Assembly == null){
                throw new Exception($"Can't load assembly of module {module.Id}");
            }
            GlobalConfigurations.Modules.Add(module);
        }
        return services;
    }

    private static void TryLoadAssemblyModule(string moduleFileFolder, ModuleInfo module)
    {
        var binary = "bin";
        var moduleBinFolder = Path.Combine(moduleFileFolder,binary);
        if(Directory.Exists(moduleBinFolder)){
            var directoryInfo = new DirectoryInfo(moduleBinFolder);
            foreach(var file in directoryInfo.GetFileSystemInfos("*.dll",SearchOption.AllDirectories)){
                if(!file.FullName.ToLower().Contains(module.Id.ToLower())){
                    continue;
                }
                Assembly assembly;
                try{
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                }catch{
                    assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));
                    if(assembly == null){
                        throw;
                    }
                    var assemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                    var fileVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;
                    if(fileVersion != assemblyVersion){
                        throw new Exception($"Can't load {file.FullName} {fileVersion} because {assembly.Location} {assemblyVersion} has been loaded!");
                    }
                }
                if(Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) == module.Id){
                    module.Assembly = assembly;
                    break;
                }
            }
        }
    }
}

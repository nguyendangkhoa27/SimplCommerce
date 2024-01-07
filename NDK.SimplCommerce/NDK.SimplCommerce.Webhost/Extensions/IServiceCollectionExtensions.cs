using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using NDK.SimplCommerce.Core.Data;
using NDK.SimplCommerce.Core.Models;
using NDK.SimplCommerce.Infrastructure;
using NDK.SimplCommerce.Infrastructure.Modules;
using NDK.SimplCommerce.Webhost.Configurations;

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

    public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services){
        services.AddIdentity<User,Role>(options => {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 0;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase =false;
        })
        .AddEntityFrameworkStores<SimplDbContext>()
        .AddDefaultTokenProviders();
        return services;
    }
    public static IServiceCollection AddCustomizedIdentityServer(this IServiceCollection services, ConfigurationManager config){
        services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        .AddInMemoryPersistedGrants()
        .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
        .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
        .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
        .AddInMemoryClients(IdentityServerConfig.GetClients())
        .AddAspNetIdentity<User>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie()
        .AddIdentityServerAuthentication(options =>{
            options.Authority = config["Identity:Authority"];
        });
        
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

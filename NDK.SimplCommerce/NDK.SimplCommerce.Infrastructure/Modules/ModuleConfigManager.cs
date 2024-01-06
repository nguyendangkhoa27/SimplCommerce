using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NDK.SimplCommerce.Infrastructure.Modules;

public class ModuleConfigManager
{
    private readonly string _modulesFileName = "modules.json";

    public IEnumerable<ModuleInfo> GetModules(){
        
        var modulesFilePath = Path.Combine(GlobalConfigurations.ContentRootPath,_modulesFileName);
        using var reader = new StreamReader(modulesFilePath);
        var content = reader.ReadToEnd();
        dynamic modules = JsonConvert.DeserializeObject(content);
        foreach(var module in modules){
            yield return new ModuleInfo(module.id.ToString(),module.name.ToString(),module.isBundledWithHost.ToString(),module.version.ToString());
        }
    }
}

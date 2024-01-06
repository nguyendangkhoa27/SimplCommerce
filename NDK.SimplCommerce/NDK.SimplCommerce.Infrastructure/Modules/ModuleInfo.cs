using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NDK.SimplCommerce.Infrastructure.Modules;

public class ModuleInfo
{
    public ModuleInfo(string id, string name, string isBundledWithHost, string version)
    {
        Id = id;
        Name = name;
        IsBundledWithHost = bool.Parse(isBundledWithHost);
        Version = Version.Parse(version);
    }

    public string Id {get;set;}
    public string Name {get;set;}
    public bool IsBundledWithHost {get;set;}
    public Version Version {get;set;}
    public Assembly Assembly {get;set;}
}

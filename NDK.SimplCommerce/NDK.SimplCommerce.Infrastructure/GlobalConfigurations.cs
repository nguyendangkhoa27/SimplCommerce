using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NDK.SimplCommerce.Infrastructure.Modules;

namespace NDK.SimplCommerce.Infrastructure;

public static class GlobalConfigurations
{
    public static string ContentRootPath {get;set;}
    public static string WebRootPath {get;set;}
    public static IList<ModuleInfo> Modules {get;set;} = new List<ModuleInfo>();
}

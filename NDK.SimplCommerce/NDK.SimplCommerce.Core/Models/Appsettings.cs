using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NDK.SimplCommerce.Infrastructure.Data;

namespace NDK.SimplCommerce.Core.Models;

public class Appsettings : EntityBaseWithTypeId<string>
{
    public string Value {get;set;}
}

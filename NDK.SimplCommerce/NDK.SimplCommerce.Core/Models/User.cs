using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NDK.SimplCommerce.Core.Models;

public class User : IdentityUser<long>
{
    public string FullName {get;set;}
}

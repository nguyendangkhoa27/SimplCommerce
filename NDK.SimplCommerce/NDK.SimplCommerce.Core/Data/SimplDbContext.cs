using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using NDK.SimplCommerce.Core.Models;
using NDK.SimplCommerce.Infrastructure;
using NDK.SimplCommerce.Infrastructure.Data;

namespace NDK.SimplCommerce.Core.Data;

public class SimplDbContext : IdentityDbContext<User,Role, long, IdentityUserClaim<long>,IdentityUserRole<long>,IdentityUserLogin<long>,IdentityRoleClaim<long>,IdentityUserToken<long>>
{
    public SimplDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var typeOfRegister = GlobalConfigurations.Modules.SelectMany(x=> x.Assembly.DefinedTypes.Select(y=> y.AsType()));
        RegisterEntity(builder,typeOfRegister);
        RegisterIdentity(builder);
        builder.Entity<Appsettings>().HasKey(x=>x.Id);
    }

    private void RegisterEntity(ModelBuilder builder, IEnumerable<Type> typeOfRegister)
    {
        var entities = typeOfRegister.Where(x=> x.GetTypeInfo().IsSubclassOf(typeof(EntityBase)) && !x.GetTypeInfo().IsAbstract);
        foreach(var entity in entities){
            builder.Entity(entity);
        }
    }
    private void RegisterIdentity(ModelBuilder builder){
        builder.Entity<User>().HasKey(x=>x.Id);
        builder.Entity<Role>().HasKey(x=>x.Id);
        builder.Entity<IdentityUserClaim<long>>().HasKey(x=>x.Id);
        builder.Entity<IdentityRoleClaim<long>>().HasKey(x=>x.Id);
        builder.Entity<IdentityUserRole<long>>().HasKey(x=> new {x.UserId,x.RoleId});
        builder.Entity<IdentityUserLogin<long>>().HasKey(x=>x.UserId);
        builder.Entity<IdentityUserToken<long>>().HasKey(x=>x.UserId);
    }
}

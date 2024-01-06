using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NDK.SimplCommerce.Core.Data;

namespace NDK.SimplCommerce.Webhost.Data;

public class SimplDbContextFactory : IDesignTimeDbContextFactory<SimplDbContext>
{
    private readonly string _appSettingName = "appsettings.json";
    private readonly string _appSettingNameEnvDev = "appsettings.Development.json";
    public SimplDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(_appSettingName)
        .AddJsonFile(_appSettingNameEnvDev)
        .Build();
        var connectionString = configuration.GetConnectionString("simplDbContext");
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<SimplDbContext>();
        optionsBuilder.UseNpgsql(connectionString,p=> p.MigrationsAssembly("NDK.SimplCommerce.Webhost"));
        return new SimplDbContext(optionsBuilder.Options);
    }
}

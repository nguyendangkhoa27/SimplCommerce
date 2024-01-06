using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NDK.SimplCommerce.Core.Data;
using NDK.SimplCommerce.Core.Models;
using NDK.SimplCommerce.Infrastructure;
using NDK.SimplCommerce.Webhost.Extensions;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; 
ConfigureServices();
var app = builder.Build();
Configure();

void ConfigureServices(){
    var services = builder.Services;
    services.AddRazorPages().AddRazorRuntimeCompilation();
    services.AddDbContext<SimplDbContext>(options => {
        var connectionString = configuration.GetConnectionString("simplDbContext");
        options.UseNpgsql(connectionString);
    });
    services.AddIdentity<User,Role>(options => {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    }).AddEntityFrameworkStores<SimplDbContext>().AddDefaultTokenProviders();
    GlobalConfigurations.ContentRootPath = builder.Environment.ContentRootPath;
    GlobalConfigurations.WebRootPath = builder.Environment.WebRootPath;
    services.AddModules();
}


void Configure(){
   if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "Core",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapAreaControllerRoute(
        name: "default",
        areaName: "Core",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
}


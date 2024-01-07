using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;

namespace NDK.SimplCommerce.Webhost.Extensions;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomizedIdentityServer(this IApplicationBuilder app){
        app.UseIdentityServer();
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/",StringComparison.OrdinalIgnoreCase)
        ,a => a.Use(async (context,next)=>
        {
            if(!context.User.Identity.IsAuthenticated){
                var principal =  new ClaimsPrincipal();
                var rs = await context.AuthenticateAsync(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                if(rs?.Principal != null){
                    principal.AddIdentities(rs.Principal.Identities);
                    context.User = principal;
                }
            }
            await next();
        }));
        app.UseAuthorization();
        return app;
    }
}

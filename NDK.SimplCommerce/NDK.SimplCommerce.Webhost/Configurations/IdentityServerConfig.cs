using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;

namespace NDK.SimplCommerce.Webhost.Configurations;

public class IdentityServerConfig
{
    public static IEnumerable<IdentityResource> GetIdentityResources(){
         return new List<IdentityResource>{
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
         };
    }
    public static IEnumerable<ApiResource> GetApiResources(){
         return new List<ApiResource>{
            new ApiResource("api","My Api")
         };
    }
    public static IEnumerable<ApiScope> GetApiScopes(){
         return new List<ApiScope>{
            new ApiScope("api","My Api")
         };
    }
    public static IEnumerable<Client> GetClients(){
         return new List<Client>{
            new Client{
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = {
                    "api", "openId","profile"
                },
                ClientSecrets ={
                    new Secret("secret".Sha256())
                }
            }
         };
    }
    
}

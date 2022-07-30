using Duende.IdentityServer.Models;
using indentityServer.Model.Enums;

namespace indentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(ScopeTypes.free.ToString()),
            new ApiScope(ScopeTypes.premium.ToString()),
        };
    public static IEnumerable<ApiResource> ApiResources =>
       new ApiResource[]
       {
            new ApiResource(name:"weatherapi"){ 
                Scopes = new string []{ScopeTypes.free.ToString(),ScopeTypes.premium.ToString() },
                ApiSecrets= new List<Secret>{ 
                    new Secret("ScopeSecret".Sha256())
                },
                UserClaims=new List<string> {"role" }
               
            },
       };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { ScopeTypes.free.ToString() }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { ScopeTypes.free.ToString(), ScopeTypes.premium.ToString() }
            },
        };
}

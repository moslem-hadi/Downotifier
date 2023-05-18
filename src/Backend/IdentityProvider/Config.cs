using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityProvider;

public static class Config
{
    public static class Constants
    {
        public const string UserRole = "user";
        public const string ApiJobsScope = "apiJobsScope";
    }

    /// <summary>
    /// An identity resource is a named group of claims about a user that can be requested using the scope parameter.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(Constants.ApiJobsScope),
        };
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
        new ApiResource("JobsWebApi")
    };
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "JobsWebApi",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha512()) },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                RedirectUris = { "http://localhost:7001/signin-oidc" },
                FrontChannelLogoutUri = "http://localhost:7001/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:7001/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    Constants.ApiJobsScope,
                    JwtClaimTypes.Role
                }
            },
        };
}

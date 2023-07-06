using IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
	public static IEnumerable<IdentityResource> IdentityResources =>
		new List<IdentityResource>
		{
			new IdentityResources.OpenId(),
			new IdentityResources.Profile(),
			new IdentityResource()
			{
				Name = "email",
				UserClaims = new List<string>
				{
					JwtClaimTypes.Email,
					JwtClaimTypes.EmailVerified
				}
			},
		};


	public static IEnumerable<ApiScope> ApiScopes =>
		new List<ApiScope>
		{
			new ApiScope("api", "RestaurantAPI")
		};

	public static IEnumerable<Client> Clients;

	public static IEnumerable<Client> SetSecrets(IConfigurationSection config)
	{
		Clients = new List<Client>
		{
			new Client
			{
				ClientId = "webapp1",
				ClientSecrets = { new Secret(config.GetValue<string>("webapp1:secret").Sha256()) },
				AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
				RedirectUris = { "https://" + config.GetValue<string>("webapp1:uri") + "/signin-oidc" }, // https://localhost:9000/signin-oidc
				PostLogoutRedirectUris = { "https://" + config.GetValue<string>("webapp1:uri") + "/signout-callback-oidc" }, // https://localhost:9000/signout-callback-oidc
				AllowOfflineAccess = true,
				AllowedScopes =
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					"api",
					"email",
				}
			},
		};

		return Clients;
	}
}
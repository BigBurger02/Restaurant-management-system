using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultScheme = "Cookies";
		options.DefaultChallengeScheme = "oidc";
	})
	.AddCookie("Cookies")
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = "https://" + builder.Configuration.GetValue<string>("ClientSecrets:IdentityServer:uri"); // https://localhost:9003

		options.ClientId = "webapp1";
		options.ClientSecret = builder.Configuration.GetValue<string>("ClientSecrets:webapp1:secret");
		options.ResponseType = "code";

		options.Scope.Clear();
		options.Scope.Add("openid");
		options.Scope.Add("profile");
		options.Scope.Add("api");
		options.Scope.Add("offline_access");

		options.Scope.Add("email");
		options.ClaimActions.MapJsonKey("email_verified", "email_verified");

		options.Scope.Add("roles");
		options.ClaimActions.MapJsonKey("role", "role", "role");
		options.TokenValidationParameters.RoleClaimType = "role";

		options.GetClaimsFromUserInfoEndpoint = true;
		options.SaveTokens = true;
	});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
app.MapRazorPages().RequireAuthorization();

app.Run();

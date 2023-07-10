using Duende.IdentityServer;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Services;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication.Twitter;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer;

internal static class HostingExtensions
{
	public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
	{
		IdentityModelEventSource.ShowPII = true;

		builder.Services.AddRazorPages();

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlite(builder.Configuration.GetConnectionString("AuthConnectionString")));

		builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		builder.Services.AddTransient<IProfileService, ProfileService>();

		builder.Services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequireDigit = true;
			options.Password.RequireUppercase = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequiredLength = 5;
			options.Password.RequiredUniqueChars = 0;

			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			options.Lockout.MaxFailedAccessAttempts = 7;
			options.Lockout.AllowedForNewUsers = true;

			options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
			options.User.RequireUniqueEmail = true;

			options.SignIn.RequireConfirmedEmail = true;
		});

		builder.Services
			.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseSuccessEvents = true;

				options.EmitStaticAudienceClaim = true;
			})
			.AddInMemoryIdentityResources(Config.IdentityResources)
			.AddInMemoryApiScopes(Config.ApiScopes)
			.AddInMemoryClients(Config.GetClients(builder.Configuration.GetSection("clientSecrets")))
			.AddAspNetIdentity<ApplicationUser>();

		builder.Services.AddAuthentication()
			.AddGoogle(options =>
			{
				options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

				options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId");
				options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret");
			})
			.AddMicrosoftAccount(microsoftOptions =>
			{
				microsoftOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

				microsoftOptions.ClientId = builder.Configuration.GetValue<string>("Authentication:Microsoft:ClientId")!;
				microsoftOptions.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Microsoft:ClientSecret")!;
			})
			.AddGitHub(githubOptions =>
			{
				githubOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

				githubOptions.ClientId = builder.Configuration.GetValue<string>("Authentication:Github:ClientId")!;
				githubOptions.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Github:ClientSecret")!;

				githubOptions.Scope.Add("user:email");
			})
			.AddTwitter(twitterOptions =>
			{
				twitterOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

				twitterOptions.ConsumerKey = builder.Configuration.GetValue<string>("Authentication:Twitter:ClientId")!;
				twitterOptions.ConsumerSecret = builder.Configuration.GetValue<string>("Authentication:Twitter:ClientSecret")!;

				twitterOptions.RetrieveUserDetails = true;
			});

		return builder.Build();
	}

	public static WebApplication ConfigurePipeline(this WebApplication app)
	{
		app.UseSerilogRequestLogging();

		//if (app.Environment.IsDevelopment())
		//{
		//	app.UseDeveloperExceptionPage();
		//}
		app.UseDeveloperExceptionPage();

		app.UseStaticFiles();
		app.UseRouting();

		app.UseIdentityServer();
		app.UseAuthorization();

		app.MapRazorPages()
			.RequireAuthorization();

		return app;
	}
}
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

public class SeedData
{
	public static void EnsureSeedData(WebApplication app)
	{
		using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
		{
			var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
			context.Database.Migrate();

			var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			#region admin
			var admin = userMgr.FindByNameAsync("admin").Result;
			if (admin == null)
			{
				admin = new ApplicationUser
				{
					UserName = "admin",
					Email = "admin@example.com",
					EmailConfirmed = true,
				};
				var result = userMgr.CreateAsync(admin, "12345").Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(admin, new Claim[]{
							new Claim(JwtClaimTypes.Name, "admin admin"),
							new Claim(JwtClaimTypes.GivenName, "admin"),
							new Claim(JwtClaimTypes.FamilyName, "admin"),
							new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("admin created");
			}
			else
			{
				Log.Debug("admin already exists");
			}
			#endregion

			#region waiter
			var waiter = userMgr.FindByNameAsync("waiter").Result;
			if (waiter == null)
			{
				waiter = new ApplicationUser
				{
					UserName = "waiter",
					Email = "waiter@example.com",
					EmailConfirmed = true,
				};
				var result = userMgr.CreateAsync(waiter, "12345").Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(waiter, new Claim[]{
							new Claim(JwtClaimTypes.Name, "waiter waiter"),
							new Claim(JwtClaimTypes.GivenName, "waiter"),
							new Claim(JwtClaimTypes.FamilyName, "waiter"),
							new Claim(JwtClaimTypes.WebSite, "http://waiter.com"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("waiter created");
			}
			else
			{
				Log.Debug("waiter already exists");
			}
			#endregion

			#region chef
			var chef = userMgr.FindByNameAsync("chef").Result;
			if (chef == null)
			{
				chef = new ApplicationUser
				{
					UserName = "chef",
					Email = "chef@example.com",
					EmailConfirmed = true,
				};
				var result = userMgr.CreateAsync(chef, "12345").Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(chef, new Claim[]{
							new Claim(JwtClaimTypes.Name, "chef chef"),
							new Claim(JwtClaimTypes.GivenName, "chef"),
							new Claim(JwtClaimTypes.FamilyName, "chef"),
							new Claim(JwtClaimTypes.WebSite, "http://chef.com"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("chef created");
			}
			else
			{
				Log.Debug("chef already exists");
			}
			#endregion

			#region cook
			var cook = userMgr.FindByNameAsync("cook").Result;
			if (cook == null)
			{
				cook = new ApplicationUser
				{
					UserName = "cook",
					Email = "cook@example.com",
					EmailConfirmed = true,
				};
				var result = userMgr.CreateAsync(cook, "12345").Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(cook, new Claim[]{
							new Claim(JwtClaimTypes.Name, "cook cook"),
							new Claim(JwtClaimTypes.GivenName, "cook"),
							new Claim(JwtClaimTypes.FamilyName, "cook"),
							new Claim(JwtClaimTypes.WebSite, "http://cook.com"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("cook created");
			}
			else
			{
				Log.Debug("cook already exists");
			}
			#endregion

			#region guest
			var guest = userMgr.FindByNameAsync("guest").Result;
			if (guest == null)
			{
				guest = new ApplicationUser
				{
					UserName = "guest",
					Email = "guest@example.com",
					EmailConfirmed = true,
				};
				var result = userMgr.CreateAsync(guest, "12345").Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(guest, new Claim[]{
							new Claim(JwtClaimTypes.Name, "guest guest"),
							new Claim(JwtClaimTypes.GivenName, "guest"),
							new Claim(JwtClaimTypes.FamilyName, "guest"),
							new Claim(JwtClaimTypes.WebSite, "http://guest.com"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("guest created");
			}
			else
			{
				Log.Debug("guest already exists");
			}
			#endregion
		}
	}
}

using System;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityServer.Data;

public class SeedData
{
	public static void EnsureSeedData(WebApplication app, string tempPassword)
	{
		using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
		{
			var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
			context.Database.Migrate();

			var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			#region roles
			if (!roleMgr.RoleExistsAsync("Admin").Result)
			{
				var result = roleMgr.CreateAsync(new IdentityRole("Admin")).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("Admin role created");
			}
			else
				Log.Debug("Admin role already exists");

			if (!roleMgr.RoleExistsAsync("Waiter").Result)
			{
				var result = roleMgr.CreateAsync(new IdentityRole("Waiter")).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("Waiter role created");
			}
			else
				Log.Debug("Waiter role already exists");

			if (!roleMgr.RoleExistsAsync("Chef").Result)
			{
				var result = roleMgr.CreateAsync(new IdentityRole("Chef")).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("Chef role created");
			}
			else
				Log.Debug("Chef role already exists");

			if (!roleMgr.RoleExistsAsync("Cook").Result)
			{
				var result = roleMgr.CreateAsync(new IdentityRole("Cook")).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("Cook role created");
			}
			else
				Log.Debug("Cook role already exists");

			if (!roleMgr.RoleExistsAsync("Guest").Result)
			{
				var result = roleMgr.CreateAsync(new IdentityRole("Guest")).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}
				Log.Debug("Guest role created");
			}
			else
				Log.Debug("Guest role already exists");
			#endregion

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
				var result = userMgr.CreateAsync(admin, tempPassword).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(admin, new Claim[]{
							new Claim(JwtClaimTypes.Name, "admin admin"),
							new Claim(JwtClaimTypes.GivenName, "admin"),
							new Claim(JwtClaimTypes.FamilyName, "admin"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddToRoleAsync(admin, "Admin").Result;
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
				var result = userMgr.CreateAsync(waiter, tempPassword).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(waiter, new Claim[]{
							new Claim(JwtClaimTypes.Name, "waiter waiter"),
							new Claim(JwtClaimTypes.GivenName, "waiter"),
							new Claim(JwtClaimTypes.FamilyName, "waiter"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddToRoleAsync(waiter, "Waiter").Result;
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
				var result = userMgr.CreateAsync(chef, tempPassword).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(chef, new Claim[]{
							new Claim(JwtClaimTypes.Name, "chef chef"),
							new Claim(JwtClaimTypes.GivenName, "chef"),
							new Claim(JwtClaimTypes.FamilyName, "chef"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddToRoleAsync(chef, "Chef").Result;
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
				var result = userMgr.CreateAsync(cook, tempPassword).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(cook, new Claim[]{
							new Claim(JwtClaimTypes.Name, "cook cook"),
							new Claim(JwtClaimTypes.GivenName, "cook"),
							new Claim(JwtClaimTypes.FamilyName, "cook"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddToRoleAsync(cook, "Cook").Result;
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
				var result = userMgr.CreateAsync(guest, tempPassword).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddClaimsAsync(guest, new Claim[]{
							new Claim(JwtClaimTypes.Name, "guest guest"),
							new Claim(JwtClaimTypes.GivenName, "guest"),
							new Claim(JwtClaimTypes.FamilyName, "guest"),
						}).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userMgr.AddToRoleAsync(guest, "Guest").Result;
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

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

using Restaurant_management_system.Infrastructure.Data.Authorization;

// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -outDir Pages\Contacts --referenceScriptLibraries
namespace Restaurant_management_system.Infrastructure.Data;

public static class AuthDbInitializer
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    #region snippet_Initialize
    public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@example.com");
            await EnsureRole(serviceProvider, adminID, RolesEnumeration.Admin);

            var waiterID = await EnsureUser(serviceProvider, testUserPw, "waiter@example.com");
            await EnsureRole(serviceProvider, waiterID, RolesEnumeration.Waiter);

            var cookID = await EnsureUser(serviceProvider, testUserPw, "cook@example.com");
            await EnsureRole(serviceProvider, cookID, RolesEnumeration.Cook);

            var chefID = await EnsureUser(serviceProvider, testUserPw, "chef@example.com");
            await EnsureRole(serviceProvider, chefID, RolesEnumeration.Chef);

            var guestID = await EnsureUser(serviceProvider, testUserPw, "guest@example.com");
            await EnsureRole(serviceProvider, guestID, RolesEnumeration.Guest);
        }
    }

    private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                string testUserPw, string UserName)
    {
        var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

        var user = await userManager.FindByNameAsync(UserName);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = UserName,
                Email = UserName,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, testUserPw);
        }

        if (user == null)
        {
            throw new Exception("User didn't created");
        }

        return user.Id;
    }

    private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                  string uid, RolesEnumeration role)
    {
        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

        if (roleManager == null)
        {
            throw new Exception("roleManager null");
        }

        IdentityResult IR;
        if (!await roleManager.RoleExistsAsync(role.ToString()))
        {
            IR = await roleManager.CreateAsync(new IdentityRole(role.ToString()));
        }

        var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

        //if (userManager == null)
        //{
        //    throw new Exception("userManager is null");
        //}

        var user = await userManager.FindByIdAsync(uid);

        if (user == null)
        {
            throw new Exception("The testUserPw password was probably not strong enough!");
        }

        IR = await userManager.AddToRoleAsync(user, role.ToString());

        return IR;
    }
    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace IdentityServer.Pages.Roles
{
	[Authorize(Roles = "Admin")]
	public class AppointRoleModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public IQueryable<IdentityRole> Roles;
		public IQueryable<ApplicationUser> Users;
		public string Info = string.Empty;

		[BindProperty]
		public string Email { get; set; }
		[BindProperty]
		public IdentityRole Role { get; set; }

		public AppointRoleModel(
		UserManager<ApplicationUser> userManager,
		RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public IActionResult OnGet(string info = "")
		{
			Info = info;

			Roles = _roleManager.Roles;

			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			var user = await _userManager.FindByEmailAsync(Email);
			if (user == null)
			{
				Info = "Error: User not found";
				return Page();
			}
			var role = await _roleManager.FindByIdAsync(Role.Id);
			if (role == null)
			{
				Info = "Error: Role not found";
				return Page();
			}

			var result = _userManager.AddToRoleAsync(user, role.Name).Result;
			if (!result.Succeeded)
			{
				Info = $"Error while assigning user to the role";
			}
			else
			{
				Info = "User assigned to the role";
			}

			return RedirectToPage("/Roles/AppointRole", new { info = Info });
		}
	}
}

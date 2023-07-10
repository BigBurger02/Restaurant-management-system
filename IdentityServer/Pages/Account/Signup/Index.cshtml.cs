// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Pages.Signup;

[AllowAnonymous]
public class IndexModel : PageModel
{
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUserStore<ApplicationUser> _userStore;
	private readonly IUserEmailStore<ApplicationUser> _emailStore;
	private readonly ILogger<IndexModel> _logger;

	public IndexModel(
		UserManager<ApplicationUser> userManager,
		IUserStore<ApplicationUser> userStore,
		SignInManager<ApplicationUser> signInManager,
		ILogger<IndexModel> logger)
	{
		_userManager = userManager;
		_userStore = userStore;
		//_emailStore = GetEmailStore();
		_signInManager = signInManager;
		_logger = logger;
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[BindProperty]
	public InputModel Input { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string ReturnUrl { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public IList<AuthenticationScheme> ExternalLogins { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public class InputModel
	{
		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}


	public async Task OnGetAsync(string returnUrl = null)
	{
		ReturnUrl = returnUrl;
		ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");

		if (ModelState.IsValid)
		{
			var user = await AutoProvisionUserAsync(Input.Email, Input.Password);

			_logger.LogInformation("User created a new account with password.");

			var userId = await _userManager.GetUserIdAsync(user);
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			var callbackUrl = Url.Page(
				"/Account/Signup/ConfirmEmail",
				pageHandler: null,
				values: new { userId = userId, code = code },
				protocol: Request.Scheme);


			string link =
				"https://localhost:9002/api/Mail?" +
				"reciever=" + Input.Email +
				"&userID=" + userId +
				"&code=" + code;

			var client = new HttpClient();

			var content = await client.GetStringAsync(link);

			if (_userManager.Options.SignIn.RequireConfirmedAccount)
			{
				return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
			}
			else
			{
				await _signInManager.SignInAsync(user, isPersistent: false);
				return LocalRedirect(returnUrl);
			}
		}

		// If we got this far, something failed, redisplay form
		return Page();
	}

	private async Task<ApplicationUser> AutoProvisionUserAsync(string email, string password)
	{
		var sub = Guid.NewGuid().ToString();

		var user = new ApplicationUser
		{
			Id = sub,
			UserName = email,
			Email = email,
		};

		var identityResult = await _userManager.CreateAsync(user, password);
		if (!identityResult.Succeeded)
			throw new Exception(identityResult.Errors.First().Description);

		return user;
	}
}

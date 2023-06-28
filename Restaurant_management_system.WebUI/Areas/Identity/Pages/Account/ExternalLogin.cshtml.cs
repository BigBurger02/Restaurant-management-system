﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_management_system.WebUI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        ILogger<ExternalLoginModel> logger,
        IEmailSender emailSender)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _logger = logger;
        _emailSender = emailSender;
    }

    [BindProperty]
    public InputModel Input { get; set; }
    public string ProviderDisplayName { get; set; }
    public string ReturnUrl { get; set; }
    [TempData]
    public string ErrorMessage { get; set; }
    [ViewData]
    public string AssociateExistingAccount { get; set; } = "false";

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public IActionResult OnGet() => RedirectToPage("./Login");

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl =
            Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties =
            _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(
        string returnUrl = null, string remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }
        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        if (result.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }
        else
        {
            // If the user does not have an account, then ask the user to create an account.
            AssociateExistingAccount = "false";
            ReturnUrl = returnUrl;
            ProviderDisplayName = info.ProviderDisplayName;
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                Input = new InputModel
                {
                    UserName = info.Principal.Identity.Name.Contains(" ") ? info.Principal.Identity.Name.Split(" ")[0] : info.Principal.Identity.Name,
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };
            }
            else if (info.Principal.Identity.Name != null && info.Principal.Identity.Name != "" && info.Principal.Identity.IsAuthenticated)
            {
                Input = new InputModel
                {
                    UserName = info.Principal.Identity.Name.Contains(" ") ? info.Principal.Identity.Name.Split(" ")[0] : info.Principal.Identity.Name,
                    Email = info.Principal.Identity.Name + "@example.com" // temporary email
                };
            }

            //// From OnPostConfirmationAsync
            var user = CreateUser();
            if (ProviderDisplayName == "GitHub" || ProviderDisplayName == "Google" || ProviderDisplayName == "Microsoft" || ProviderDisplayName == "Twitter")
                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
            else
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var createResult = await _userManager.CreateAsync(user);

            if (createResult.Errors.Count() != 0 && createResult.Errors.First().Code == "InvalidUserName")
            {
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                createResult = await _userManager.CreateAsync(user);
            }

            if (createResult.Succeeded)
            {
                createResult = await _userManager.AddLoginAsync(user, info);
                if (createResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by " +
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>" +
                        $"clicking here</a>.");
                    // If account confirmation is required, we need to show the link if
                    // we don't have a real email sender
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                    return LocalRedirect(returnUrl);
                }
            }
            else
            {
                // There is an existing account 
                // Check if that account has a password 
                var ExistingUserToCheck = await _userManager.FindByEmailAsync(user.Email);
                if (ExistingUserToCheck != null)
                {
                    if (ExistingUserToCheck.PasswordHash == null)
                    {
                        StringBuilder PasswordNotSetError = new StringBuilder();
                        PasswordNotSetError.Append("There is an existing account with that email address. ");
                        PasswordNotSetError.Append("However, that account has no password set. ");
                        PasswordNotSetError.Append("Please log in to that account, with the ");
                        PasswordNotSetError.Append("existing external login method, and set a password. ");
                        PasswordNotSetError.Append("Then you can associate it with additional external ");
                        PasswordNotSetError.Append("login methods.");
                        AssociateExistingAccount = "blocked";
                        ModelState.AddModelError(string.Empty, PasswordNotSetError.ToString());
                        return Page();
                    }
                }
                // We can associate this login to the existing account
                AssociateExistingAccount = "true";
            }
            // Display any errors that occurred
            // Usually says email is already used
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //// From OnPostConfirmationAsync

            return Page();
        }
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
        //returnUrl = returnUrl ?? Url.Content("~/");
        //// Get the information about the user from the external login provider
        //var info = await _signInManager.GetExternalLoginInfoAsync();
        //if (info == null)
        //{
        //    ErrorMessage = "Error loading external login information during confirmation.";
        //    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        //}
        //if (ModelState.IsValid)
        //{
        //    var user = CreateUser();
        //    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        //    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        //    var result = await _userManager.CreateAsync(user);
        //    if (result.Succeeded)
        //    {
        //        result = await _userManager.AddLoginAsync(user, info);
        //        if (result.Succeeded)
        //        {
        //            var userId = await _userManager.GetUserIdAsync(user);
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { area = "Identity", userId = userId, code = code },
        //                protocol: Request.Scheme);
        //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                $"Please confirm your account by " +
        //                $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>" +
        //                $"clicking here</a>.");
        //            // If account confirmation is required, we need to show the link if
        //            // we don't have a real email sender
        //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //            {
        //                return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
        //            }
        //            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
        //            return LocalRedirect(returnUrl);
        //        }
        //    }
        //    else
        //    {
        //        // There is an existing account 
        //        // Check if that account has a password 
        //        var ExistingUserToCheck = await _userManager.FindByEmailAsync(user.Email);
        //        if (ExistingUserToCheck != null)
        //        {
        //            if (ExistingUserToCheck.PasswordHash == null)
        //            {
        //                StringBuilder PasswordNotSetError = new StringBuilder();
        //                PasswordNotSetError.Append("There is an existing account with that email address. ");
        //                PasswordNotSetError.Append("However, that account has no password set. ");
        //                PasswordNotSetError.Append("Please log in to that account, with the ");
        //                PasswordNotSetError.Append("existing external login method, and set a password. ");
        //                PasswordNotSetError.Append("Then you can associate it with additional external ");
        //                PasswordNotSetError.Append("login methods.");
        //                AssociateExistingAccount = "blocked";
        //                ModelState.AddModelError(string.Empty, PasswordNotSetError.ToString());
        //                return Page();
        //            }
        //        }
        //        // We can associate this login to the existing account
        //        AssociateExistingAccount = "true";
        //    }
        //    // Display any errors that occurred
        //    // Usually says email is already used
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}
        //ProviderDisplayName = info.ProviderDisplayName;
        //ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAssociateLoginAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        // Set AssociateExistingAccount so we return to this method on postback
        AssociateExistingAccount = "true";
        // Get the information about the user from the external login provider
        var ExternalLoginUser = await _signInManager.GetExternalLoginInfoAsync();
        if (ExternalLoginUser == null)
        {
            ErrorMessage = "Error loading external login information during confirmation.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }
        if (Input.Password != null)
        {
            try
            {
                // Get email of the ExternalLoginUser
                string ExternalLoginUserEmail = "";
                if (ExternalLoginUser.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    ExternalLoginUserEmail =
                        ExternalLoginUser.Principal.FindFirstValue(ClaimTypes.Email);
                }
                // Check password against user in database
                var user = await _userManager.FindByEmailAsync(ExternalLoginUserEmail);
                if (user != null)
                {
                    var CheckPasswordResult =
                        await _userManager.CheckPasswordAsync(user, Input.Password);
                    if (CheckPasswordResult)
                    {
                        // user found and password is correct
                        // add external login to user and sign in
                        var AddLoginResult =
                            await _userManager.AddLoginAsync(user, ExternalLoginUser);
                        if (AddLoginResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            foreach (var error in AddLoginResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else // password is incorrect
                    {
                        ModelState.AddModelError(string.Empty, "Password is incorrect");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Password is required");
        }
        // If we got this far, something failed, redisplay form
        return Page();
    }

    private IdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of " +
                $"'{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class " +
                $"and has a parameterless constructor, or alternatively " +
                $"override the external login page in " +
                $"/Areas/Identity/Pages/Account/ExternalLogin.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException(
                "The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)_userStore;
    }
}
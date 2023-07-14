using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;

namespace WebClient.Controllers;

[Authorize]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	[AllowAnonymous]
	public IActionResult Index()
	{
		return View();
	}
	[Authorize(Roles = "Admin")]
	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	[AllowAnonymous]
	public IActionResult SetCulture(string culture, string returnUrl)
	{
		Response.Cookies.Append(
			CookieRequestCultureProvider.DefaultCookieName,
			CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
			new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
		);
		return LocalRedirect(returnUrl);
	}
}

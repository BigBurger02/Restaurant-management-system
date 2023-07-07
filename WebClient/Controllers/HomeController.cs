using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Controllers;

[Authorize]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	[Authorize(Roles = "Admin")]
	public IActionResult Index()
	{
		ViewData["roles"] = User.IsInRole("Admin").ToString() + User.IsInRole("Waiter").ToString();
		return View();
	}
	[AllowAnonymous]
	public IActionResult Privacy()
	{
		return View();
	}

	public async Task<IActionResult> UserInfo()
	{
		var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() };
		if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
		{
			return NotFound();
		}

		var userInfo = new UserInfo(await HttpContext.AuthenticateAsync());

		return View(userInfo);
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}

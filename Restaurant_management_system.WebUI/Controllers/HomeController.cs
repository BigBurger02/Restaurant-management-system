using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Docs.Samples;

using Restaurant_management_system.WebUI.ViewModels;
using Restaurant_management_system.Core.Services.Logger;

namespace Restaurant_management_system.WebUI.Controllers;

[Authorize]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IStringLocalizer<HomeController> _localizer;

	public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
	{
		_logger = logger;
		_localizer = localizer;
	}

	[AllowAnonymous]
	public IActionResult Index()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		return View();
	}

	[AllowAnonymous]
	public IActionResult Privacy()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		return View();
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public IActionResult Analytics()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		throw new NotImplementedException();
	}

	[AllowAnonymous]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	[Authorize(Roles = "Admin")]
	public IActionResult Test()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		return View();
	}
}


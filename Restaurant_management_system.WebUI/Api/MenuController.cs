using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Docs.Samples;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Core.Services.Attributes;
using Restaurant_management_system.Core.Services.Logger;
using Restaurant_management_system.Core.Interfaces;

namespace Restaurant_management_system.WebUI.Api;

[Authorize]
[Route("api/[controller]")]
[SwaggerControllerOrder(1)]
public class MenuController : Controller
{
	private readonly ILogger<MenuController> _logger;
	private readonly IRestaurantRepository _context;
	private readonly IStringLocalizer<MenuController> _localizer;

	public MenuController(ILogger<MenuController> logger, IRestaurantRepository context, IStringLocalizer<MenuController> localizer)
	{
		_logger = logger;
		_context = context;
		_localizer = localizer;
	}

	/// <summary>
	/// Get all dishes
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     GET api/menu
	/// </remarks>
	/// <response code="200">Returns all menu items</response>
	/// <response code="204">Menu is empty</response>
	// GET: api/menu
	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(204)]
	[Produces("application/json")]
	public ActionResult<List<DishItemDTO>> GetMenu()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		var dishes = _context.GetAllDishesFromMenu();
		if (dishes == null)
			return StatusCode(500, "Dish list is empty");

		return Ok(dishes);
	}
}
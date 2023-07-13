using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

	public MenuController(ILogger<MenuController> logger, IRestaurantRepository context)
	{
		_logger = logger;
		_context = context;
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
	/// <response code="500">Menu is empty</response>
	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(204)]
	[Produces("application/json")]
	public ObjectResult GetMenu()
	{
		_logger.LogInformation(LogEvents.VisitMethod, "MenuController/GetMenu visited at {time}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), LogEvents.VisitMethod);

		var dishes = _context.GetAllDishesFromMenu();
		if (dishes == null)
			return StatusCode(500, "Menu is empty");

		return new ObjectResult(dishes) { StatusCode = 200 };
	}
}
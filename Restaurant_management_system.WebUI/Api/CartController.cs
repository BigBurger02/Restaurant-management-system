using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Restaurant_management_system.Core.Services.Attributes;
using Restaurant_management_system.Core.Services.Logger;
using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.WebUI.ViewModels;

namespace Restaurant_management_system.WebUI.Api;

[Authorize]
[Route("api/[controller]")]
[SwaggerControllerOrder(3)]
public class CartController : Controller
{
	private readonly ILogger<CartController> _logger;
	private readonly IRestaurantRepository _context;

	public CartController(ILogger<CartController> logger, IRestaurantRepository context)
	{
		_logger = logger;
		_context = context;
	}

	/// <summary>
	/// Get all dishes from order
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     GET api/Cart/4
	/// </remarks>
	/// <response code="200">Returns all cart items or returns nothing if order is empty</response>
	/// <response code="404">Order not found</response>
	[HttpGet("{orderID}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	[Produces("application/json")]
	public ObjectResult GetCart(int orderID)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "CartController/GetCart visited at {time}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), LogEvents.VisitMethod);

		var order = _context.FindOrderByID(orderID);
		if (order == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in OrderInTable table. LogEvent:{logevent}", orderID, LogEvents.NotFoundInDB);
			return NotFound($"Order {orderID} not found");
		}

		var cart = _context.GetAllDishesFromOrder(orderID);
		if (cart == null)
			return new ObjectResult(null) { StatusCode = 200 };

		return new ObjectResult(cart) { StatusCode = 200 };
	}

	/// <summary>
	/// Add one dish to the specified order
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     POST api/Cart/4/3
	/// </remarks>
	/// <param name="orderID"></param>
	/// <param name="dishIdInMenu"></param>
	/// <response code="204">Dish added to the order</response>
	/// <response code="404">Item not found</response>
	/// <response code="500">Server error</response>
	[HttpPost("{orderID}/{dishIdInMenu}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	[Produces("application/json")]
	public ObjectResult AddToCart(int orderID, int dishIdInMenu)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "CartController/AddToCart visited at {time}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), LogEvents.VisitMethod);

		var order = _context.FindOrderByID(orderID);
		if (order == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in OrderInTable table. LogEvent:{logevent}", orderID, LogEvents.NotFoundInDB);
			return NotFound($"Order {orderID} not found");
		}

		var dish = _context.FindDishInMenuById(dishIdInMenu);
		if (dish == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in DishInMenu table. LogEvent:{logevent}", dishIdInMenu, LogEvents.NotFoundInDB);
			return NotFound($"Dish {dishIdInMenu} not found");
		}

		var newDish = _context.CreateDishInOrder(orderID, dishIdInMenu);
		if (newDish == null || newDish.ID == 0)
			return StatusCode(500, "Server error");

		_logger.LogInformation(LogEvents.CreateInDB, "Item {item} changed in DishInOrder table. LogEvent:{logevent}", newDish.ID, LogEvents.CreateInDB);

		return StatusCode(204, "Dish added");
	}

	/// <summary>
	/// Delete one dish from the specified order
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     DELETE api/Cart/4/3
	/// </remarks>
	/// <param name="orderID"></param>
	/// <param name="dishIdInMenu"></param>
	/// <response code="204">Dish removed from the order</response>
	/// <response code="404">Item not found</response>
	[HttpDelete("{orderID}/{dishIdInMenu}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(404)]
	[Produces("application/json")]
	public ObjectResult DeleteFromCart(int orderID, int dishIdInMenu)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "CartController/DeleteFromCart visited at {time}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), LogEvents.VisitMethod);

		var order = _context.FindOrderByID(orderID);
		if (order == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in OrderInTable table. LogEvent:{logevent}", orderID, LogEvents.NotFoundInDB);
			return NotFound($"Order {orderID} not found");
		}

		var dishInMenu = _context.FindDishInMenuById(dishIdInMenu);
		if (dishInMenu == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in DishInMenu table. LogEvent:{logevent}", dishIdInMenu, LogEvents.NotFoundInDB);
			return NotFound($"Dish {dishIdInMenu} not found");
		}

		var dishInOrder = _context.FindDishInOrderByIdInMenu(orderID, dishIdInMenu);
		if (dishInOrder == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in DishInOrder table. LogEvent:{logevent}", dishIdInMenu, LogEvents.NotFoundInDB);
			return NotFound($"Dish {dishIdInMenu} not found");
		}

		var result = _context.RemoveDishFromOrderByID(orderID, dishIdInMenu);

		_logger.LogInformation(LogEvents.RemoveFromDB, "Item {item} changed in DishInOrder table. LogEvent:{logevent}", dishIdInMenu, LogEvents.RemoveFromDB);

		return StatusCode(204, "Dish deleted");
	}
}


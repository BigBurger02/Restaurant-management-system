using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Restaurant_management_system.Core.Services.Attributes;
using Restaurant_management_system.Core.Services.Logger;
using Restaurant_management_system.Core.Interfaces;

namespace Restaurant_management_system.WebUI.Api;

[Authorize]
[Route("api/[controller]")]
[SwaggerControllerOrder(2)]
public class OrderController : Controller
{
	private readonly ILogger<OrderController> _logger;
	private readonly IRestaurantRepository _context;

	public OrderController(ILogger<OrderController> logger, IRestaurantRepository context)
	{
		_logger = logger;
		_context = context;
	}

	/// <summary>
	/// Open new order for the specified table
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     POST api/order/5
	/// </remarks>
	/// <param name="tableID"></param>
	/// <response code="404">Table not found</response>
	/// <response code="201">Returns the orderID of created order. Order connected to given tableID</response>
	/// <response code="500">Server error</response>
	[HttpPost("{tableID}")]
	[ProducesResponseType(404)]
	[ProducesResponseType(201)]
	[ProducesResponseType(500)]
	[Produces("application/json")]
	public ObjectResult CreateOrder(int tableID)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "OrderController/CreateOrder visited at {time}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), LogEvents.VisitMethod);

		var table = _context.FindTableByID(tableID);
		if (table == null)
		{
			_logger.LogInformation(LogEvents.NotFoundInDB, "Item {item} not found in Table table. LogEvent:{logevent}", tableID, LogEvents.NotFoundInDB);
			return NotFound($"Table {tableID} not found");
		}

		var newOrder = _context.CreateOrder(tableID);

		if (newOrder.ID == 0)
			return StatusCode(500, "Server error");

		_logger.LogInformation(LogEvents.CreateInDB, "Item {item} created in OrderInTable table. LogEvent:{logevent}", newOrder.ID, LogEvents.CreateInDB);

		return Created("", newOrder.ID);
	}

	/// <summary>
	/// Close order
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     PATCH api/order/4
	/// </remarks>
	/// <param name="orderID"></param>
	/// <response code="204">Order closed successfuly</response>
	/// <response code="400">Error</response>
	[HttpPatch("{orderID}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[Produces("application/json")]
	public ActionResult CloseOrder(int orderID)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "OrderController/CloseOrder visited at {time}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), LogEvents.VisitMethod);

		if (!_context.CloseOrderByID(orderID))
			return BadRequest();

		_logger.LogInformation(LogEvents.SetDataInDB, "Item {item} changed in OrderInTable table. LogEvent:{logevent}", orderID, LogEvents.SetDataInDB);

		return NoContent();
	}
}
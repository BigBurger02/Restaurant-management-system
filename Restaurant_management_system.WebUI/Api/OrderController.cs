using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Core.Services.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Restaurant_management_system.WebUI.Api;

[AllowAnonymous]
[Route("api/[controller]")]
[SwaggerControllerOrder(2)]
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly RestaurantContext _context;
    private readonly IStringLocalizer<OrderController> _localizer;

    public OrderController(ILogger<OrderController> logger, RestaurantContext context, IStringLocalizer<OrderController> localizer)
    {
        _logger = logger;
        _context = context;
        _localizer = localizer;
    }

    /// <summary>
    /// Open new order for the specified table
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT api/order/5
    /// </remarks>
    /// <param name="tableID"></param>
    /// <response code="404">Table not found</response>
    /// <response code="201">Returns the orderID of created order. Order connected to given tableID</response>
    /// <response code="500">Server error</response>
    // PUT: api/order/5
    [HttpPut("{tableID}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(201)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    public ActionResult<int> CreateOrder(int tableID)
    {
        var table = _context.Table
            .Find(tableID);
        if (table == null)
            return NotFound($"Table {tableID} not found");

        var newOrder = new OrderInTableEntity() { TableID = tableID, SelfOrdered = true };
        _context.OrderInTable.Add(newOrder);
        _context.SaveChanges();
        _context.Entry(newOrder).GetDatabaseValues();

        if (newOrder.ID == 0)
            return StatusCode(500, "Server error");

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
    /// <response code="404">Order not found</response>
    // PATCH: api/order/4
    [HttpPatch("{orderID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [Produces("application/json")]
    public ActionResult CloseOrder(int orderID)
    {
        var order = _context.OrderInTable
            .Find(orderID);
        if (order == null)
            return NotFound($"Order {orderID} not found");

        order.Open = false;

        return NoContent();
    }
}


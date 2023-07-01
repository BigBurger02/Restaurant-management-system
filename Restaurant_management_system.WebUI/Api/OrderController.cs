using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Core.Services.Attributes;

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
    /// Open new order for the specified table (in development)
    /// </summary>
    /// <returns>int</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT api/order/5
    /// </remarks>
    /// <param name="tableID"></param>
    /// <response code="201">Returns the orderID. Order connected to given tableID</response>
    /// <response code="400">Can't find that table (not implemented)</response>
    // PUT: api/order/5
    [HttpPut("{tableID}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Produces("application/json")]
    public int CreateOrder(int tableID)
    {
        var newOrder = new OrderInTableEntity() { TableID = tableID, SelfOrdered = true };
        _context.OrderInTable.Add(newOrder);
        _context.SaveChanges();
        _context.Entry(newOrder).GetDatabaseValues();

        return newOrder.ID;
    }

    /// <summary>
    /// Close order (in development)
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PATCH api/order/4
    /// </remarks>
    /// <param name="orderID"></param>
    /// <response code="201">Order closed successfuly</response>
    /// <response code="400">Some problems (not implemented)</response>
    // PATCH: api/order/4
    [HttpPatch("{orderID}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Produces("application/json")]
    public bool CloseOrder(int orderID)
    {
        var order = _context.OrderInTable
            .Find(orderID);
        order.Open = false;

        return true;
    }
}


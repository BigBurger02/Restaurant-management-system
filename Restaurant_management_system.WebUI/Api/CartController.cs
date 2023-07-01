using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.SwaggerGen;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Core.Services.Attributes;

namespace Restaurant_management_system.WebUI.Api;

[AllowAnonymous]
[Route("api/[controller]")]
[SwaggerControllerOrder(3)]
public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly RestaurantContext _context;
    private readonly IStringLocalizer<CartController> _localizer;

    public CartController(ILogger<CartController> logger, RestaurantContext context, IStringLocalizer<CartController> localizer)
    {
        _logger = logger;
        _context = context;
        _localizer = localizer;
    }

    /// <summary>
    /// Add one dish to the specified order (in development)
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PATCH api/Cart/5/4/3
    /// </remarks>
    /// <param name="tableID"></param>
    /// <param name="orderID"></param>
    /// <param name="dishID"></param>
    /// <response code="201">Dish added to the order (not implemented)</response>
    /// <response code="400">Some problems (not implemented)</response>
    // PATCH: api/cart/5/4/3
    [HttpPatch("{tableID}/{orderID}/{dishID}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Produces("application/json")]
    public bool AddToCart(int tableID, int orderID, int dishID)
    {
        var table = _context.Table
            .Find(tableID);
        var order = _context.OrderInTable
            .Find(orderID);
        var dish = _context.DishInMenu
            .Find(dishID);

        var newDish = new DishInOrderEntity() { OrderID = orderID, DishID = dishID };
        _context.DishInOrder.Add(newDish);
        _context.SaveChanges();

        return true;
    }

    /// <summary>
    /// Delete one dish from the specified order (in development)
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE api/Cart/5/4/3
    /// </remarks>
    /// <param name="tableID"></param>
    /// <param name="orderID"></param>
    /// <param name="dishID"></param>
    /// <response code="201">One dish removed from the order (not implemented)</response>
    /// <response code="400">Some problems (not implemented)</response>
    // DELETE: api/cart/5/4/3
    [HttpDelete("{tableID}/{orderID}/{dishID}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Produces("application/json")]
    public bool DeleteFromCart(int tableID, int orderID, int dishID)
    {
        var table = _context.Table
            .Find(tableID);
        var order = _context.OrderInTable
            .OrderByDescending(o => o.ID)
            .First(order => order.ID == orderID);
        var dish = _context.DishInOrder
            .First(d => d.OrderID == orderID && d.DishID == dishID);

        _context.DishInOrder.Remove(dish);
        _context.SaveChanges();

        return true;
    }
}


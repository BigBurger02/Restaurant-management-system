using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.WebUI.ApiModels;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.WebUI.Api;

[AllowAnonymous]
[Route("api/[controller]")]
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

    // PATCH: api/cart/5/5/5
    [HttpPatch("{tableID}/{orderID}/{dishID}")]
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

    // DELETE: api/cart/5/5/5
    [HttpDelete("{tableID}/{orderID}/{dishID}")]
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


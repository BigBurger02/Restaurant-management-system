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

    // PUT: api/visitor/5
    [HttpPut("{tableID}")]
    public int CreateOrder(int tableID)
    {
        var newOrder = new OrderInTableEntity() { TableID = tableID, SelfOrdered = true };
        _context.OrderInTable.Add(newOrder);
        _context.SaveChanges();
        _context.Entry(newOrder).GetDatabaseValues();

        return newOrder.ID;
    }

    // PATCH: api/visitor/5
    [HttpPatch("{orderID}")]
    public bool CloseOrder(int orderID)
    {
        var order = _context.OrderInTable
            .Find(orderID);
        order.Open = false;

        return true;
    }
}


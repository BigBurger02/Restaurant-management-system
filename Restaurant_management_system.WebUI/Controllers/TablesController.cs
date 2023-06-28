using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.WebUI.ViewModels;

namespace Restaurant_management_system.WebUI.Controllers;

[Authorize]
public class TablesController : Controller
{
    private readonly ILogger<TablesController> _logger;
    private readonly RestaurantContext _context;
    private readonly IStringLocalizer<TablesController> _localizer;

    public TablesController(ILogger<TablesController> logger, RestaurantContext context, IStringLocalizer<TablesController> localizer)
    {
        _logger = logger;
        _context = context;
        _localizer = localizer;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult Tables()
    {
        string localizedTrue = _localizer["Yes"];
        string localizedFalse = _localizer["No"];

        var tables = _context.Table
            .AsNoTracking()
            .Select(table => new TableDTO
            {
                ID = table.ID,
                IsOccupiedString = table.IsOccupied ? localizedTrue : localizedFalse,
                IsPaidString = table.IsPaid ? localizedTrue : localizedFalse,
                AmountOfGuests = table.AmountOfGuests,
                OrderCost = 0
            })
            .ToList();

        return View(tables);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult EditTableAndOrder(int tableID)
    {
        string localizedTrue = _localizer["Yes"];
        string localizedFalse = _localizer["No"];

        var findTable = _context.Table
            .AsNoTracking()
            .FirstOrDefault(i => i.ID == tableID);

        var findOrder = _context.OrderInTable
            .AsNoTracking()
            .OrderByDescending(o => o.ID)
            .First(table => table.TableID == tableID);

        var findDishes = _context.DishInOrder
            .AsNoTracking()
            .Where(order => order.OrderID == findOrder.ID)
            .Select(dish => new DishInOrderDTO
            {
                ID = dish.ID,
                DishName = _context.DishInMenu.FirstOrDefault(i => i.ID == dish.DishID).Name.ToString(),
                TimeOfOrderingString = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
                IsDoneString = dish.IsDone ? localizedTrue : localizedFalse,
                IsTakenAwayString = dish.IsTakenAway ? localizedTrue : localizedFalse,
                IsPrioritizedString = dish.IsPrioritized ? localizedTrue : localizedFalse
            })
            .ToList();


        var table = new TableDTO()
        {
            ID = findTable.ID,
            IsOccupiedBool = findTable.IsOccupied,
            IsPaidBool = findTable.IsPaid,
            IsOccupiedString = findTable.IsOccupied ? localizedTrue : localizedFalse,
            IsPaidString = findTable.IsPaid ? localizedTrue : localizedFalse,
            AmountOfGuests = findTable.AmountOfGuests,
            OrderCost = findTable.OrderCost,
            Order = new OrderInTableDTO()
        };

        table.Order.ID = findOrder.ID;
        table.Order.TableID = findOrder.TableID;
        table.Order.Open = findOrder.Open;
        table.Order.Message = findOrder.Message;
        foreach (var item in findDishes)
            table.Order.Dishes.Add(item);

        return View(table);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult EditTableAndOrder([Bind("ID,IsOccupiedBool,IsPaidBool,AmountOfGuests,Order")] TableDTO inputTable)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("EditTableAndOrder", new { tableID = inputTable.ID });

        var findTable = _context.Table
            .Find(inputTable.ID);

        findTable.Order = _context.OrderInTable
            .OrderByDescending(o => o.ID)
            .First(table => table.TableID == inputTable.ID);

        findTable.IsOccupied = inputTable.IsOccupiedBool;
        findTable.IsPaid = inputTable.IsPaidBool;
        findTable.AmountOfGuests = inputTable.AmountOfGuests;
        findTable.Order.Message = inputTable.Order.Message;

        _context.SaveChanges();

        return RedirectToAction("EditTableAndOrder", new { tableID = inputTable.ID });
    }

    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult ResetTable(int? tableID)
    {
        var table = _context.Table
            .Where(t => t.ID == tableID)
            .FirstOrDefault();

        if (table != null)
        {
            table.IsOccupied = false;
            table.AmountOfGuests = 0;
            table.OrderCost = 0;
            table.IsPaid = false;

            var findOrder = _context.OrderInTable
                .OrderBy(table => table.TableID)
                .LastOrDefault(table => table.TableID == tableID);
            findOrder.Open = false;
            table.Order = new Core.TablesAggregate.OrderInTableEntity(table.ID);
            _context.OrderInTable.Add(table.Order);

            _context.SaveChanges();

            return RedirectToAction("Tables", "Tables");
        }
        else
            return RedirectToAction("Error", "Home");

    }

    [HttpGet]
    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult EditDishInOrder(int? dishID, int tableID)
    {
        var dish = _context.DishInOrder
            .AsNoTracking()
            .FirstOrDefault(d => d.ID == dishID);

        var dishWithTypes = new DishInOrderDTO
        {
            ID = dish.ID,
            OrderID = dish.OrderID,
            TimeOfOrderingString = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
            IsDoneBool = dish.IsDone,
            IsTakenAwayBool = dish.IsTakenAway,
            IsPrioritizedBool = dish.IsPrioritized,
            TableID = tableID
        };
        if (dish.DishID == 0)
            dishWithTypes.DishName = _localizer["empty"];
        else
        {
            dishWithTypes.DishInMenuID = dish.DishID;
            dishWithTypes.DishName = _context.DishInMenu.FirstOrDefault(i => i.ID == dish.DishID).Name.ToString();
        }

        var menuDTOs = _context.DishInMenu
            .AsNoTracking()
            .Select(i => new DishInMenuDTO
            {
                ID = i.ID,
                Name = i.Name
            })
            .ToList();

        foreach (var item in menuDTOs)
            dishWithTypes.DishDTOs.Add(item);

        return View(dishWithTypes);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult EditDishInOrder([Bind("ID,IsTakenAwayBool,IsPrioritizedBool")] DishInOrderDTO inputDish, int tableID, int DishInMenuID)
    {
        var dishEntity = _context.DishInOrder
            .Find(inputDish.ID);

        dishEntity.DishID = DishInMenuID;
        dishEntity.IsTakenAway = inputDish.IsTakenAwayBool;
        dishEntity.IsPrioritized = inputDish.IsPrioritizedBool;

        _context.SaveChanges();

        return RedirectToAction("EditTableAndOrder", new { tableID = tableID });
    }

    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult AddDishInOrder(int orderID, int tableID)
    {
        var newdish = new DishInOrderEntity() { OrderID = orderID, };
        _context.DishInOrder.Add(newdish);
        _context.SaveChanges();

        int dishID = _context.DishInOrder
            .AsNoTracking()
            .OrderByDescending(d => d.ID)
            .First(order => order.OrderID == orderID)
            .ID;

        return RedirectToAction("EditDishInOrder", new { dishID = dishID, tableID = tableID });
    }
}


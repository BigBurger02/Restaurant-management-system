using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.WebUI.ViewModels;
using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Restaurant_management_system.WebUI.Controllers;

public class TablesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RestaurantContext _context;

    public TablesController(ILogger<HomeController> logger, RestaurantContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Tables()
    {
        var tables = _context.Table
            .AsNoTracking()
            .Select(table => new TableDTO
            {
                ID = table.ID,
                IsOccupied = table.IsOccupied ? "Yes" : "No",
                IsPaid = table.IsPaid ? "Yes" : "No",
                AmountOfGuests = table.AmountOfGuests,
                OrderCost = table.OrderCost
            })
            .ToList();

        return View(tables);
    }

    [HttpGet]
    public IActionResult EditTable(int? tableID)
    {
        var tableQuery = _context.Table
            .AsNoTracking()
            .First(t => t.ID == tableID);

        TableWithTypesDTO table = new TableWithTypesDTO
        {
            ID = tableQuery.ID,
            IsOccupied = tableQuery.IsOccupied,
            IsPaid = tableQuery.IsPaid,
            AmountOfGuests = tableQuery.AmountOfGuests
        };

        return View(table);
    }

    [HttpPost]
    public IActionResult EditTable([Bind("ID,IsOccupied,IsPaid,AmountOfGuests")] TableWithTypesDTO inputTable)
    {
        var tableQuery = _context.Table
            .First(t => t.ID == inputTable.ID);

        tableQuery.IsOccupied = inputTable.IsOccupied;
        tableQuery.IsPaid = inputTable.IsPaid;
        tableQuery.AmountOfGuests = inputTable.AmountOfGuests;

        TableWithTypesDTO table = new TableWithTypesDTO
        {
            ID = tableQuery.ID,
            IsOccupied = inputTable.IsOccupied,
            IsPaid = inputTable.IsPaid,
            AmountOfGuests = inputTable.AmountOfGuests
        };

        _context.SaveChanges();

        return RedirectToAction("Tables", "Tables");
    }

    [HttpGet]
    public IActionResult EditOrder(int? tableID)
    {
        var findOrder = _context.Order
            .AsNoTracking()
            .OrderByDescending(o => o.ID)
            .First(table => table.TableID == tableID);

        var findDishes = _context.Dish
            .AsNoTracking()
            .Where(order => order.OrderID == findOrder.ID)
            .Select(dish => new DishDTO
            {
                DishID = dish.ID,
                DishName = dish.DishName,
                TimeOfOrdering = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
                IsDone = dish.IsDone ? "Yes" : "No",
                IsTakenAway = dish.IsTakenAway ? "Yes" : "No",
                IsPrioritized = dish.IsPrioritized ? "Yes" : "No"
            })
            .ToList();

        ViewData["OrderID"] = findOrder.ID;
        ViewData["TableID"] = findOrder.TableID;
        ViewData["Message"] = findOrder.Message;

        return View(findDishes);
    }

    [HttpPost]
    public IActionResult EditOrder(int tableID, int orderID, string message)
    {
        var order = _context.Order
            .First(i => i.ID == orderID);
        order.Message = message;
        _context.SaveChanges();

        return RedirectToAction("EditOrder", new { tableID = tableID });
    }

    [HttpGet]
    public IActionResult EditDish(int? dishID, int tableID)
    {
        var dish = _context.Dish
            .AsNoTracking()
            .FirstOrDefault(d => d.ID == dishID);

        var dishWithTypes = new DishWithTypesDTO
        {
            ID = dish.ID,
            OrderID = dish.OrderID,
            TimeOfOrdering = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
            IsDone = dish.IsDone,
            IsTakenAway = dish.IsTakenAway,
            IsPrioritized = dish.IsPrioritized,
            TableID = tableID
        };
        dishWithTypes.DishName = _context.Menu
            .AsNoTracking()
            .First(d => d.Name == dish.DishName)
            .ID;

        return View(dishWithTypes);
    }

    [HttpPost]
    public IActionResult EditDish([Bind("ID,OrderID,DishName,TimeOfOrdering,IsDone,IsTakenAway,IsPrioritized")] DishWithTypesDTO inputDish, int tableID)
    {
        var dish = _context.Dish
            .FirstOrDefault(d => d.ID == inputDish.ID);

        dish.DishName = _context.Menu.
            First(n => n.ID == inputDish.DishName)
            .Name;
        dish.IsTakenAway = inputDish.IsTakenAway;
        dish.IsPrioritized = inputDish.IsPrioritized;

        _context.SaveChanges();

        return RedirectToAction("EditOrder", new { tableID = tableID });
    }

    public IActionResult AddDish(int orderID, int tableID)
    {
        var newdish = new DishEntity() { OrderID = orderID };
        _context.Dish.Add(newdish);
        _context.SaveChanges();

        int dishID = _context.Dish
            .AsNoTracking()
            .OrderByDescending(d => d.ID)
            .First(order => order.OrderID == orderID)
            .ID;

        return RedirectToAction("EditDish", new { dishID = dishID, tableID = tableID });
    }

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

            var findOrder = _context.Order
                .OrderBy(table => table.TableID)
                .LastOrDefault(table => table.TableID == tableID);
            findOrder.Open = false;
            table.Order = new Core.TablesAggregate.OrderEntity(table.ID);
            _context.Order.Add(table.Order);

            _context.SaveChanges();

            return RedirectToAction("Tables", "Tables");
        }
        else
            return RedirectToAction("Error", "Home");

    }

    [HttpGet]
    public IActionResult Menu()
    {
        var menu = _context.Menu
            .AsNoTracking()
            .Select(item => new MenuDTO
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                IngredientsNames = item.IngredientsID
            })
            .ToList();

        foreach (var item in menu)
        {
            string[] IngredientsIdTmp = item.IngredientsNames.Split(",");
            string IngredientsNamesTmp = string.Empty;

            foreach (var ingredient in IngredientsIdTmp)
            {
                var nameTmp = _context.Ingredient
                    .AsNoTracking()
                    .FirstOrDefault(id => id.ID == int.Parse(ingredient));

                IngredientsNamesTmp += nameTmp.Name + ", ";
            }

            item.IngredientsNames = IngredientsNamesTmp.Remove(IngredientsNamesTmp.Length - 2);
        }

        return View(menu);
    }
}


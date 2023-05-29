using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.WebUI.ViewModels;

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
    public IActionResult EditOrder(int? id)
    {
        var findOrder = _context.Order
            .AsNoTracking()
            .OrderBy(table => table.TableID)
            .LastOrDefault(table => table.TableID == id);

        var findDishes = _context.Dish
            .AsNoTracking()
            .Where(order => order.OrderID == findOrder.ID)
            .Select(dish => new DishDTO
            {
                DishID = dish.ID,
                DishName = dish.DishName,
                TimeOfOrdering = dish.DateOfOrdering.Split(" ", StringSplitOptions.None)[1],
                IsDone = dish.IsDone ? "Yes" : "No",
                IsTakenAway = dish.IsTakenAway ? "Yes" : "No",
                IsPrioritized = dish.IsPrioritized ? "Yes" : "No"
            });

        OrderDTO order = new OrderDTO
        {
            ID = findOrder.ID,
            TableID = findOrder.TableID,
            Message = findOrder.Message
        };

        foreach (var item in findDishes)
        {
            if (item != null)
                order.Dishes.Add(item);
        }

        return View(order);
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


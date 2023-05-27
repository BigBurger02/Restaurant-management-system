using Restaurant_management_system.Infrastructure.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurant_management_system.WebUI.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_management_system.WebUI.Controllers;

public class DishesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RestaurantContext _context;

    public DishesController(ILogger<HomeController> logger, RestaurantContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult DishesList()
    {
        var listOfDishes = _context.Dish
            .AsNoTracking()
            .Select(dish => new DishDTO
            {
                DishID = dish.ID,
                DishName = dish.DishName,
                TimeOfOrdering = dish.DateOfOrdering.Split(" ", StringSplitOptions.None)[1],
                IsDone = dish.IsDone ? "Yes" : "No",
                IsTakenAway = dish.IsTakenAway ? "Yes" : "No",
                IsPrioritized = dish.IsPrioritized ? "Yes" : "No"
            })
            .ToList();

        return View(listOfDishes);
    }
}


using Restaurant_management_system.Infrastructure.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurant_management_system.WebUI.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_management_system.WebUI.Controllers;

public class KitchenController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RestaurantContext _context;

    public KitchenController(ILogger<HomeController> logger, RestaurantContext context)
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
                TimeOfOrdering = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
                IsDone = dish.IsDone ? "Yes" : "No",
                IsTakenAway = dish.IsTakenAway ? "Yes" : "No",
                IsPrioritized = dish.IsPrioritized ? "Yes" : "No"
            })
            .ToList();

        return View(listOfDishes);
    }

    [HttpGet]
    public IActionResult SupplyOfProducts()
    {
        var ingredients = _context.Ingredient
            .AsNoTracking()
            .Select(ingredient => new IngredientDTO
            {
                ID = ingredient.ID,
                Name = ingredient.Name,
                Price = ingredient.Price
            })
            .ToList();

        return View(ingredients);
    }

    [HttpGet]
    public IActionResult Cooks()
    {
        throw new NotImplementedException();
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.WebUI.ViewModels;

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
        var listOfDishes = _context.DishInOrder
            .AsNoTracking()
            .Select(dish => new DishInOrderDTO
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
    public IActionResult EditIngredient(int? ingredientID)
    {
        if (ingredientID == null)
            return View();

        var findIngredient = _context.Ingredient
            .AsNoTracking()
            .FirstOrDefault(i => i.ID == ingredientID);

        var ingredient = new IngredientDTO
        {
            ID = findIngredient.ID,
            Name = findIngredient.Name,
            Price = findIngredient.Price
        };

        return View(ingredient);
    }

    [HttpPost]
    public IActionResult EditIngredient([Bind("ID,Name,Price")] IngredientDTO inputIngredient)
    {
        if (inputIngredient.ID == 0)
        {
            var newingredient = new IngredientEntity
            {
                Name = inputIngredient.Name,
                Price = inputIngredient.Price
            };
            _context.Ingredient.Add(newingredient);
            _context.SaveChanges();

            return RedirectToAction("SupplyOfProducts", "Kitchen");
        }

        var ingredient = _context.Ingredient
            .Find(inputIngredient.ID);

        ingredient.Name = inputIngredient.Name;
        ingredient.Price = inputIngredient.Price;
        _context.SaveChanges();

        return RedirectToAction("SupplyOfProducts", "Kitchen");
    }

    public IActionResult RemoveIngredient(int ingredientID)
    {
        var ingredient = _context.Ingredient
            .Find(ingredientID);

        _context.Ingredient.Remove(ingredient);
        _context.SaveChanges();

        return RedirectToAction("SupplyOfProducts", "Kitchen");
    }

    [HttpGet]
    public IActionResult Cooks()
    {
        throw new NotImplementedException();
    }
}


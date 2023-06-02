﻿using Microsoft.AspNetCore.Mvc;
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
    public IActionResult ActualDishes()
    {
        var listOfDishes = _context.DishInOrder
            .AsNoTracking()
            .Where(d => d.DateOfOrdering.Date == DateTime.Today.Date && d.IsDone == false)
            .Select(dish => new DishInOrderDTO
            {
                DishID = dish.ID,
                DishName = dish.DishName,
                TimeOfOrderingString = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
                IsDoneString = dish.IsDone ? "Yes" : "No",
                IsTakenAwayString = dish.IsTakenAway ? "Yes" : "No",
                IsPrioritizedString = dish.IsPrioritized ? "Yes" : "No"
            })
            .ToList();

        return View(listOfDishes);
    }

    public IActionResult Change_DONE_InDish(int dishID)
    {
        var dish = _context.DishInOrder
            .Find(dishID);

        if (dish.IsDone == true)
            dish.IsDone = false;
        else
            dish.IsDone = true;

        _context.SaveChanges();

        return RedirectToAction("ActualDishes", "Kitchen");
    }

    [HttpGet]
    public IActionResult Ingredients()
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

            return RedirectToAction("Ingredients", "Kitchen");
        }

        var ingredient = _context.Ingredient
            .Find(inputIngredient.ID);

        ingredient.Name = inputIngredient.Name;
        ingredient.Price = inputIngredient.Price;
        _context.SaveChanges();

        return RedirectToAction("Ingredients", "Kitchen");
    }

    public IActionResult RemoveIngredient(int ingredientID)
    {
        var ingredient = _context.Ingredient
            .Find(ingredientID);

        _context.Ingredient.Remove(ingredient);
        _context.SaveChanges();

        return RedirectToAction("Ingredients", "Kitchen");
    }

    [HttpGet]
    public IActionResult Cooks()
    {
        throw new NotImplementedException();
    }
}

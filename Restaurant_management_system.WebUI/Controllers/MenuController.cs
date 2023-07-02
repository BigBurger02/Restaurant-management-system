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
public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly RestaurantContext _context;
    private readonly IStringLocalizer<MenuController> _localizer;

    public MenuController(ILogger<MenuController> logger, RestaurantContext context, IStringLocalizer<MenuController> localizer)
    {
        _logger = logger;
        _context = context;
        _localizer = localizer;
    }

    [Authorize(Roles = "Admin, Waiter, Chef")]
    [HttpGet]
    public IActionResult DishesInMenu()
    {
        var menu = _context.DishInMenu
            .AsNoTracking()
            .Select(item => new DishInMenuDTO
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price
            })
            .ToList();

        // Ingredients:
        foreach (var oneMenuEntity in menu)
        {
            var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.DishInMenuID == oneMenuEntity.ID);

            foreach (var oneMenuInredientsEntity in ingredientsID.ToList())
            {
                var ingredientEntity = _context.Ingredient
                    .AsNoTracking()
                    .FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID);
                if (ingredientEntity == null)
                    continue;

                oneMenuEntity.IngredientsNames += ingredientEntity.Name + ", ";
            }
            if (ingredientsID.Any())
            {
                oneMenuEntity.IngredientsNames = oneMenuEntity.IngredientsNames.Remove(oneMenuEntity.IngredientsNames.Length - 2);// Remove 2 last symbols: ", "
            }
        }

        return View(menu);
    }

    [Authorize(Roles = "Admin, Chef")]
    [HttpGet]
    public IActionResult EditDishInMenu(int menuID)
    {
        var menuEntity = _context.DishInMenu
            .AsNoTracking()
            .FirstOrDefault(i => i.ID == menuID);
        if (menuEntity == null)
            return NotFound();

        var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.DishInMenuID == menuEntity.ID)
                .ToList();

        var ingredientsDTO = new List<IngredientDTO>();

        foreach (var oneMenuInredientsEntity in ingredientsID)
        {
            var ingredientEntity = _context.Ingredient
                .AsNoTracking()
                .FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID);
            if (ingredientEntity == null)
                continue;

            ingredientsDTO.Add(new IngredientDTO
            {
                ID = ingredientEntity.ID,
                Name = ingredientEntity.Name
            });
        }

        ViewData["MenuID"] = menuID;
        ViewData["MenuName"] = menuEntity.Name;
        ViewData["MenuPrice"] = menuEntity.Price;

        return View(ingredientsDTO);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Chef")]
    public IActionResult EditDishInMenu(int menuID, string menuName, int menuPrice)
    {
        var menuEntity = _context.DishInMenu
            .Find(menuID);
        if (menuEntity == null)
            return NotFound();

        menuEntity.Name = menuName;
        menuEntity.Price = menuPrice;

        _context.SaveChanges();

        return RedirectToAction("EditDishInMenu", new { menuID });
    }

    [Authorize(Roles = "Admin, Chef")]
    public IActionResult AddDishInMenu()
    {
        var newMenuEntity = new DishInMenuEntity();

        _context.DishInMenu.Add(newMenuEntity);
        _context.SaveChanges();

        return RedirectToAction("EditDishInMenu", new { menuID = newMenuEntity.ID });
    }

    [Authorize(Roles = "Admin")]
    public IActionResult RemoveDishInMenu(int menuID)
    {
        var menuEntity = _context.DishInMenu
            .Find(menuID);
        if (menuEntity == null)
            return NotFound();

        var menuIngredientsEntity = _context.IngredientForDishInMenu
            .Where(i => i.DishInMenuID == menuID)
            .AsEnumerable();

        if (menuIngredientsEntity.Any())
            _context.IngredientForDishInMenu
                .RemoveRange(menuIngredientsEntity);

        _context.DishInMenu
            .Remove(menuEntity);

        _context.SaveChanges();

        return RedirectToAction("DishesInMenu", "Menu");
    }

    [Authorize(Roles = "Admin, Chef")]
    public IActionResult RemoveIngredientInDish(int ingredientID, int menuID)
    {
        var menuIngredientEntity = _context.IngredientForDishInMenu
            .FirstOrDefault(m => m.DishInMenuID == menuID && m.IngredientID == ingredientID);
        if (menuIngredientEntity == null)
            return NotFound();

        _context.IngredientForDishInMenu.Remove(menuIngredientEntity);

        _context.SaveChanges();

        return RedirectToAction("EditDishInMenu", new { menuID = menuID });
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Chef")]
    public IActionResult AddIngredientInDish(int menuID)
    {
        var ingredients = _context.Ingredient
            .Select(i => new IngredientDTO
            {
                ID = i.ID,
                Name = i.Name
            })
            .ToList();

        ViewData["MenuID"] = menuID;

        return View(ingredients);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Chef")]
    public IActionResult AddIngredientInDish(int ingredientID, int menuID)
    {
        var newMenuIngredient = new IngredientForDishInMenuEntity()
        {
            DishInMenuID = menuID,
            IngredientID = ingredientID
        };

        _context.IngredientForDishInMenu.Add(newMenuIngredient);

        _context.SaveChanges();

        return RedirectToAction("EditDishInMenu", new { menuID = menuID });
    }
}
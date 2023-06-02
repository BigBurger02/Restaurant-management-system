using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
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
                OrderCost = 0
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
        var findOrder = _context.OrderInTable
            .AsNoTracking()
            .OrderByDescending(o => o.ID)
            .First(table => table.TableID == tableID);

        var findDishes = _context.DishInOrder
            .AsNoTracking()
            .Where(order => order.OrderID == findOrder.ID)
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

        ViewData["OrderID"] = findOrder.ID;
        ViewData["TableID"] = findOrder.TableID;
        ViewData["Message"] = findOrder.Message;

        return View(findDishes);
    }

    [HttpPost]
    public IActionResult EditOrder(int tableID, int orderID, string message)
    {
        var order = _context.OrderInTable
            .First(i => i.ID == orderID);
        order.Message = message;
        _context.SaveChanges();

        return RedirectToAction("EditOrder", new { tableID = tableID });
    }

    [HttpGet]
    public IActionResult EditDish(int? dishID, int tableID)
    {
        var dish = _context.DishInOrder
            .AsNoTracking()
            .FirstOrDefault(d => d.ID == dishID);

        var dishWithTypes = new DishWithTypesDTO
        {
            ID = dish.ID,
            OrderID = dish.OrderID,
            DishName = dish.DishName,
            TimeOfOrdering = dish.DateOfOrdering.Hour.ToString("D2") + ":" + dish.DateOfOrdering.Minute.ToString("D2"),
            IsDone = dish.IsDone,
            IsTakenAway = dish.IsTakenAway,
            IsPrioritized = dish.IsPrioritized,
            TableID = tableID
        };

        if (dish.DishName != "")
            dishWithTypes.MenuID = _context.DishInMenu
                .FirstOrDefault(i => i.Name == dish.DishName)
                .ID;
        else
            dishWithTypes.MenuID = 0;

        var menuDTOs = _context.DishInMenu
            .AsNoTracking()
            .Select(i => new DishInMenuMenuDTO
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
    public IActionResult EditDish([Bind("ID,IsTakenAway,IsPrioritized,DishName")] DishWithTypesDTO inputDish, int tableID, int MenuID)
    {
        var dishEntity = _context.DishInOrder
            .Find(inputDish.ID);

        dishEntity.DishName = _context.DishInMenu
            .AsNoTracking()
            .FirstOrDefault(i => i.ID == MenuID)
            .Name;
        dishEntity.IsTakenAway = inputDish.IsTakenAway;
        dishEntity.IsPrioritized = inputDish.IsPrioritized;

        _context.SaveChanges();

        return RedirectToAction("EditOrder", new { tableID = tableID });
    }

    public IActionResult AddDish(int orderID, int tableID)
    {
        var newdish = new DishInOrderEntity() { OrderID = orderID, };
        _context.DishInOrder.Add(newdish);
        _context.SaveChanges();

        int dishID = _context.DishInOrder
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
    public IActionResult Menu()
    {
        var menu = _context.DishInMenu
            .AsNoTracking()
            .Select(item => new DishInMenuMenuDTO
            {
                ID = item.ID,
                Name = item.Name
            })
            .ToList();

        foreach (var oneMenuEntity in menu)
        {
            var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.MenuID == oneMenuEntity.ID);

            foreach (var oneMenuInredientsEntity in ingredientsID)
            {
                var ingredientEntity = _context.Ingredient
                    .AsNoTracking()
                    .FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID);

                oneMenuEntity.Price += ingredientEntity.Price;
                oneMenuEntity.IngredientsNames += ingredientEntity.Name + ", ";
            }
            if (ingredientsID.Count() != 0)
            {
                oneMenuEntity.IngredientsNames = oneMenuEntity.IngredientsNames.Remove(oneMenuEntity.IngredientsNames.Length - 2);// Remove 2 last symbols: ", "
                oneMenuEntity.Price += ((oneMenuEntity.Price * 50) / 100);// Add 50% to price
            }
        }

        return View(menu);
    }

    [HttpGet]
    public IActionResult EditMenu(int menuID)
    {
        var menuEntity = _context.DishInMenu
            .AsNoTracking()
            .FirstOrDefault(i => i.ID == menuID);

        var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.MenuID == menuEntity.ID);

        var ingredientsDTO = new List<IngredientDTO>();

        foreach (var oneMenuInredientsEntity in ingredientsID)
        {
            var ingredientEntity = _context.Ingredient
                .AsNoTracking()
                .FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID);

            ingredientsDTO.Add(new IngredientDTO
            {
                ID = ingredientEntity.ID,
                Name = ingredientEntity.Name
            });
        }

        ViewData["MenuID"] = menuID;
        ViewData["MenuName"] = menuEntity.Name;

        return View(ingredientsDTO);
    }

    [HttpPost]
    public IActionResult EditMenu(int menuID, string menuName)
    {
        var menuEntity = _context.DishInMenu
            .Find(menuID);

        menuEntity.Name = menuName;

        _context.SaveChanges();

        return RedirectToAction("EditMenu", new { menuID = menuID });
    }

    public IActionResult AddMenu()
    {
        var newMenuEntity = new DishInMenuEntity()
        {
            Name = "No name"
        };

        _context.DishInMenu.Add(newMenuEntity);
        _context.SaveChanges();

        int menuID = _context.DishInMenu
            .AsNoTracking()
            .FirstOrDefault(n => n.Name == "No name")
            .ID;

        return RedirectToAction("EditMenu", new { menuID = menuID });
    }

    public IActionResult RemoveMenu(int menuID)
    {
        var menuEntity = _context.DishInMenu
            .Find(menuID);

        var menuIngredientsEntity = _context.IngredientForDishInMenu
            .Where(i => i.MenuID == menuID)
            .AsEnumerable();

        _context.IngredientForDishInMenu
            .RemoveRange(menuIngredientsEntity);

        _context.DishInMenu
            .Remove(menuEntity);

        _context.SaveChanges();

        return RedirectToAction("Menu", "Tables");
    }

    public IActionResult RemoveMenuIngredient(int ingredientID, int menuID)
    {
        var menuIngredientEntity = _context.IngredientForDishInMenu
            .FirstOrDefault(m => m.MenuID == menuID && m.IngredientID == ingredientID);

        _context.IngredientForDishInMenu.Remove(menuIngredientEntity);

        _context.SaveChanges();

        return RedirectToAction("EditMenu", new { menuID = menuID });
    }

    [HttpGet]
    public IActionResult AddMenuIngredient(int menuID)
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
    public IActionResult AddMenuIngredient(int ingredientID, int menuID)
    {
        var newMenuIngredient = new IngredientForDishInMenuEntity()
        {
            MenuID = menuID,
            IngredientID = ingredientID
        };

        _context.IngredientForDishInMenu.Add(newMenuIngredient);

        _context.SaveChanges();

        return RedirectToAction("EditMenu", new { menuID = menuID });
    }
}


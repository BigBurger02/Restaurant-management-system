using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.WebUI.ViewModels;

namespace Restaurant_management_system.WebUI.Controllers;

[Authorize]
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
    [Authorize(Roles = "Admin, Waiter")]
    public IActionResult Tables()
    {
        var tables = _context.Table
            .AsNoTracking()
            .Select(table => new TableDTO
            {
                ID = table.ID,
                IsOccupiedString = table.IsOccupied ? "Yes" : "No",
                IsPaidString = table.IsPaid ? "Yes" : "No",
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
                IsDoneString = dish.IsDone ? "Yes" : "No",
                IsTakenAwayString = dish.IsTakenAway ? "Yes" : "No",
                IsPrioritizedString = dish.IsPrioritized ? "Yes" : "No"
            })
            .ToList();


        var table = new TableDTO()
        {
            ID = findTable.ID,
            IsOccupiedBool = findTable.IsOccupied,
            IsPaidBool = findTable.IsPaid,
            IsOccupiedString = findTable.IsOccupied ? "Yes" : "No",
            IsPaidString = findTable.IsPaid ? "Yes" : "No",
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
            dishWithTypes.DishName = "No name";
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

    [Authorize(Roles = "Admin, Waiter, Chef")]
    [HttpGet]
    public IActionResult DishesInMenu()
    {
        var menu = _context.DishInMenu
            .AsNoTracking()
            .Select(item => new DishInMenuDTO
            {
                ID = item.ID,
                Name = item.Name
            })
            .ToList();

        foreach (var oneMenuEntity in menu)
        {
            var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.DishInMenuID == oneMenuEntity.ID);

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

    [Authorize(Roles = "Admin, Chef")]
    [HttpGet]
    public IActionResult EditDishInMenu(int menuID)
    {
        var menuEntity = _context.DishInMenu
            .AsNoTracking()
            .FirstOrDefault(i => i.ID == menuID);

        var ingredientsID = _context.IngredientForDishInMenu
                .AsNoTracking()
                .Where(i => i.DishInMenuID == menuEntity.ID);

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
    [Authorize(Roles = "Admin, Chef")]
    public IActionResult EditDishInMenu(int menuID, string menuName)
    {
        var menuEntity = _context.DishInMenu
            .Find(menuID);

        menuEntity.Name = menuName;

        _context.SaveChanges();

        return RedirectToAction("EditDishInMenu", new { menuID = menuID });
    }

    [Authorize(Roles = "Admin, Chef")]
    public IActionResult AddDishInMenu()
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

        return RedirectToAction("EditDishInMenu", new { menuID = menuID });
    }

    [Authorize(Roles = "Admin")]
    public IActionResult RemoveDishInMenu(int menuID)
    {
        var menuEntity = _context.DishInMenu
            .Find(menuID);

        var menuIngredientsEntity = _context.IngredientForDishInMenu
            .Where(i => i.DishInMenuID == menuID)
            .AsEnumerable();

        _context.IngredientForDishInMenu
            .RemoveRange(menuIngredientsEntity);

        _context.DishInMenu
            .Remove(menuEntity);

        _context.SaveChanges();

        return RedirectToAction("DishesInMenu", "Tables");
    }

    [Authorize(Roles = "Admin, Chef")]
    public IActionResult RemoveIngredientInDish(int ingredientID, int menuID)
    {
        var menuIngredientEntity = _context.IngredientForDishInMenu
            .FirstOrDefault(m => m.DishInMenuID == menuID && m.IngredientID == ingredientID);

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


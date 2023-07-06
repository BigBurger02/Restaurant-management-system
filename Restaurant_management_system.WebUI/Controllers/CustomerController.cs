using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.WebUI.ViewModels;

namespace Restaurant_management_system.WebUI.Controllers;

//[AllowAnonymous]
public class CustomerController : Controller
{
	private readonly ILogger<CustomerController> _logger;
	private readonly RestaurantContext _context;
	private readonly IStringLocalizer<CustomerController> _localizer;

	public CustomerController(ILogger<CustomerController> logger, RestaurantContext context, IStringLocalizer<CustomerController> localizer)
	{
		_logger = logger;
		_context = context;
		_localizer = localizer;
	}

	public IActionResult Menu(int orderID)
	{
		var menu = _context.DishInMenu
			.AsNoTracking()
			.Select(dish => new DishItemDTO
			{
				ID = dish.ID,
				Name = dish.Name,
				Price = dish.Price
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

				oneMenuEntity.IngredientsForCustomer += ingredientEntity.Name + ", ";
			}
			if (ingredientsID.Count() != 0)
			{
				oneMenuEntity.IngredientsForCustomer = oneMenuEntity.IngredientsForCustomer.Remove(oneMenuEntity.IngredientsForCustomer.Length - 2);// Remove 2 last symbols: ", "
			}
		}

		ViewData["orderID"] = orderID;
		return View(menu);
	}

	public IActionResult AddToCart(int orderID, int dishID)
	{
		if (orderID == 0)
		{
			var newOrder = new OrderInTableEntity();
			_context.OrderInTable.Add(newOrder);
			_context.SaveChanges();
			_context.Entry(newOrder).GetDatabaseValues();
			orderID = newOrder.ID;
		}

		var order = _context.OrderInTable
			.FirstOrDefault(o => o.ID == orderID);
		if (order == null)
			return NotFound();

		order.SelfOrdered = true;

		var newDish = new DishInOrderEntity();
		newDish.DishID = dishID;
		newDish.OrderID = orderID;
		_context.DishInOrder.Add(newDish);
		_context.SaveChanges();

		return RedirectToAction("Menu", "Customer", new { orderID = orderID });
	}

	public IActionResult Cart(int orderID)
	{
		if (orderID != 0)
		{
			var dishesFromOrder = _context.DishInOrder
				.AsNoTracking()
				.Where(d => d.OrderID == orderID)
				.Select(dish => new DishItemDTO
				{
					ID = dish.DishID
				})
				.ToList();

			foreach (var item in dishesFromOrder)
			{
				var dish = _context.DishInMenu
					.Find(item.ID);
				if (dish == null)
					return NotFound();

				item.Name = dish.Name;
				item.Price = dish.Price;
			}

			ViewData["orderID"] = orderID;
			return View(dishesFromOrder);
		}

		return View();
	}

	public IActionResult RemoveFromCart(int orderID, int dishID)
	{
		var dishInOrder = _context.DishInOrder
			.Where(o => o.OrderID == orderID && o.DishID == dishID)
			.FirstOrDefault();
		if (dishInOrder == null)
			return NotFound();

		_context.Remove(dishInOrder);
		_context.SaveChanges();

		return RedirectToAction("Cart", "Customer", new { orderID = orderID });
	}

	public IActionResult AddToCartFromCart(int orderID, int dishID)
	{
		var newDish = new DishInOrderEntity() { OrderID = orderID, DishID = dishID };
		_context.DishInOrder.Add(newDish);
		_context.SaveChanges();

		return RedirectToAction("Cart", "Customer", new { orderID = orderID });
	}

	public IActionResult Pay()
	{
		throw new NotImplementedException();
	}
}


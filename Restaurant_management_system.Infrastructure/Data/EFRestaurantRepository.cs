using System;
using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Microsoft.EntityFrameworkCore;
using MailKit.Search;

namespace Restaurant_management_system.Infrastructure.Data;

public class EFRestaurantRepository : IRestaurantRepository
{
	private readonly RestaurantContext _context;

	public EFRestaurantRepository(RestaurantContext context)
	{
		_context = context;
	}

	#region Tables
	public async Task<List<TableEntity>> TablesToListAsync()
	{
		var table = await _context.Table
			.AsNoTracking()
			.ToListAsync();
		return table;
	}

	public TableEntity FindTableByID(int tableID)
	{
		var table = _context.Table
			.Find(tableID);
		return table!;
	}
	#endregion

	#region Order
	public OrderInTableEntity FindOrderByID(int orderID)
	{
		var order = _context.OrderInTable
			.Find(orderID);
		return order!;
	}

	public OrderInTableEntity CreateOrder(int tableID)
	{
		var newOrder = new OrderInTableEntity() { TableID = tableID, SelfOrdered = true };
		_context.OrderInTable.Add(newOrder);
		_context.SaveChanges();

		return newOrder;
	}

	public bool CloseOrderByID(int orderID)
	{
		var order = FindOrderByID(orderID);
		if (order == null)
			return false;

		order.Open = false;
		_context.SaveChanges();

		return true;
	}
	#endregion

	#region Dishes In Menu
	public List<DishInMenuEntity> GetAllDishesFromMenuWithIngredients()
	{
		var dishes = _context.DishInMenu
			.AsNoTracking()
			.Select(d => new DishInMenuEntity
			{
				ID = d.ID,
				Name = d.Name,
				Price = d.Price
			})
			.ToList();

		if (!dishes.Any())
			return null!;

		// Ingredients:
		foreach (var oneMenuEntity in dishes)
		{
			var ingredientsID = _context.IngredientForDishInMenu
				.AsNoTracking()
				.Where(i => i.DishInMenuID == oneMenuEntity.ID)
				.ToList();

			if (oneMenuEntity.Ingredients != null)
				foreach (var oneMenuInredientsEntity in ingredientsID)
				{
					var ingredient = _context.Ingredient
						.AsNoTracking()
						.FirstOrDefault(i => i.ID == oneMenuInredientsEntity.IngredientID);
					if (ingredient == null)
						continue;

					oneMenuEntity.Ingredients.Add(ingredient);
				}
		}

		return dishes;
	}

	public List<DishInMenuEntity> GetAllDishesFromMenu()
	{
		var dishes = _context.DishInMenu
			.AsNoTracking()
			.Select(d => new DishInMenuEntity
			{
				ID = d.ID,
				Name = d.Name,
				Price = d.Price
			})
			.ToList();

		if (!dishes.Any())
			return null!;

		return dishes;
	}

	public DishInMenuEntity FindDishInMenuById(int dishId)
	{
		var dish = _context.DishInMenu
			.Find(dishId);
		return dish!;
	}
	#endregion

	#region Dishes In Order
	public DishInOrderEntity FindDishInOrderByIdInMenu(int orderID, int dishIdInMenu)
	{
		var dish = _context.DishInOrder
			.Where(d => d.OrderID == orderID && d.DishID == dishIdInMenu)
			.FirstOrDefault();

		return dish!;
	}

	public DishInOrderEntity CreateDishInOrder(int orderID, int dishIdInMenu)
	{
		var newDish = new DishInOrderEntity() { OrderID = orderID, DishID = dishIdInMenu };
		_context.DishInOrder.Add(newDish);
		_context.SaveChanges();

		return newDish;
	}

	public bool RemoveDishFromOrderByID(int orderID, int dishIdInMenu)
	{
		var dish = FindDishInOrderByIdInMenu(orderID, dishIdInMenu);
		if (dish == null)
		{
			return false;
		}

		_context.DishInOrder.Remove(dish);
		_context.SaveChanges();
		return true;
	}
	#endregion
}
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
	public TableEntity FindTableByID(int tableID)
	{
		var table = _context.Table
			.Find(tableID);
		return table;
	}
	#endregion

	#region Order
	public OrderInTableEntity FindOrderByID(int orderID)
	{
		var order = _context.OrderInTable
			.Find(orderID);
		return order;
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
			return null;

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
			return null;

		return dishes;
	}
	#endregion

	#region Dishes In Order
	public DishInOrderEntity FindDishInOrderByID(int dishID)
	{
		var dish = _context.DishInOrder
			.Find(dishID);

		return dish;
	}

	public DishInOrderEntity CreateDishInOrder(int orderID, int dishID)
	{
		var newDish = new DishInOrderEntity() { OrderID = orderID, DishID = dishID };
		_context.DishInOrder.Add(newDish);
		_context.SaveChanges();

		return newDish;
	}

	public bool RemoveDishFromOrderByID(int dishID)
	{
		var dish = FindDishInOrderByID(dishID);
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
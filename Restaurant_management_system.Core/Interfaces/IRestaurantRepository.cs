using System;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.Core.Interfaces;

public interface IRestaurantRepository
{
	List<DishInMenuEntity> GetAllDishesFromMenuWithIngredients();
	List<DishInMenuEntity> GetAllDishesFromMenu();

	TableEntity FindTableByID(int tableID);

	OrderInTableEntity FindOrderByID(int orderID);
	OrderInTableEntity CreateOrder(int tableID);
	bool CloseOrderByID(int orderID);

	DishInOrderEntity FindDishInOrderByID(int dishID);
	DishInOrderEntity CreateDishInOrder(int orderID, int dishID);
	bool RemoveDishFromOrderByID(int dishID);
}
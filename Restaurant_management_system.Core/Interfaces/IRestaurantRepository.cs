using System;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.Core.Interfaces;

public interface IRestaurantRepository
{
	List<DishInMenuEntity> GetAllDishesFromMenuWithIngredients();
	List<DishInMenuEntity> GetAllDishesFromMenu();
	DishInMenuEntity FindDishInMenuById(int dishId);

	Task<List<TableEntity>> TablesToListAsync();
	TableEntity FindTableByID(int tableID);

	OrderInTableEntity FindOrderByID(int orderID);
	List<DishesDTO> GetAllDishesFromOrder(int orderID);
	OrderInTableEntity CreateOrder(int tableID);
	bool CloseOrderByID(int orderID);

	DishInOrderEntity FindDishInOrderByIdInMenu(int orderID, int dishIdInMenu);
	DishInOrderEntity CreateDishInOrder(int orderID, int dishIdInMenu);
	bool RemoveDishFromOrderByID(int orderID, int dishID);
}
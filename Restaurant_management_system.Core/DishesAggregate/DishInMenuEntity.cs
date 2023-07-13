using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class DishInMenuEntity
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Price { get; set; }

	public List<IngredientEntity> Ingredients { get; set; }

	public DishInMenuEntity()
	{
		Name = string.Empty;
		Price = 0;
		Ingredients = new List<IngredientEntity>();
	}
}
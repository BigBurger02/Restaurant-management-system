using Restaurant_management_system.Core.DishesAggregate;

namespace Restaurant_management_system.WebUI.ViewModels;

public class DishWithIngredientsListDTO
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Price { get; set; }

	public List<IngredientEntity> Ingredients { get; set; }

	public DishWithIngredientsListDTO()
	{
		Name = string.Empty;
		Price = 0;

		Ingredients = new List<IngredientEntity>();
	}
}


using Bogus;

namespace Restaurant_management_system.WebUI.Tests;

public class DataGenerator
{
	public DataGenerator(int amountIngredients = NumberOfIngredients, int amountDishesInMenu = NumbreOfDishesInMenu)
	{
		GenerateBogusData(amountIngredients, amountDishesInMenu);
	}

	public List<IngredientEntity> Ingredients = new List<IngredientEntity>();
	public List<DishInMenuEntity> DishesInMenu = new List<DishInMenuEntity>();

	private const int NumbreOfDishesInMenu = 10;
	private const int NumberOfIngredients = 40;

	public void GenerateBogusData(int amountIngredients = NumberOfIngredients, int amountDishesInMenu = NumbreOfDishesInMenu)
	{
		Ingredients.AddRange(GetIngredientGenerator().Generate(amountIngredients));
		DishesInMenu.AddRange(GetDishInMenuGenerator().Generate(amountDishesInMenu));
	}

	private Faker<IngredientEntity> GetIngredientGenerator()
	{
		return new Faker<IngredientEntity>()
			.RuleFor(v => v.ID, f => f.IndexFaker)
			.RuleFor(v => v.Name, f => f.Lorem.Word())
			.RuleFor(v => v.Price, f => int.Parse(f.Commerce.Price(decimals: 0)));
	}

	private Faker<DishInMenuEntity> GetDishInMenuGenerator()
	{
		return new Faker<DishInMenuEntity>()
			.RuleFor(v => v.ID, f => f.IndexFaker)
			.RuleFor(v => v.Name, f => f.Lorem.Word())
			.RuleFor(v => v.Price, f => int.Parse(f.Commerce.Price(decimals: 0)))
			.RuleFor(v => v.Ingredients, c =>
			{
				var rand = new Random();
				var ingredients = new List<IngredientEntity>();
				var amount = Ingredients.Count() > 6 ? 6 : Ingredients.Count();
				for (int i = 0; i < rand.Next(2, amount); i++)
					ingredients.Add(c.PickRandom(Ingredients));
				return ingredients;
			});
	}
}
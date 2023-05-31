using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class IngredientEntity
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public IngredientEntity()
    {
        Name = string.Empty;
        Price = 0;
    }
}
using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class IngredientForDishInMenuEntity
{
    public int ID { get; set; }
    public int DishInMenuID { get; set; }
    public int IngredientID { get; set; }

    public IngredientForDishInMenuEntity()
    {
    }
}
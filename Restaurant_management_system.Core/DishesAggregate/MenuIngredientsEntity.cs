using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class MenuIngredientsEntity
{
    public int ID { get; set; }
    public int MenuID { get; set; }
    public int IngredientID { get; set; }

    public MenuIngredientsEntity()
    {
    }
}
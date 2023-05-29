using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class MenuEntity
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public string IngredientsID { get; set; } = string.Empty;
}
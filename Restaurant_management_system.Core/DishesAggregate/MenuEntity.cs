using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class MenuEntity
{
    public int ID { get; set; }
    public string Name { get; set; }

    public MenuEntity()
    {
        Name = string.Empty;
    }
}
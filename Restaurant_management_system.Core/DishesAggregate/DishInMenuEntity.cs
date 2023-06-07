using System;

namespace Restaurant_management_system.Core.DishesAggregate;

public class DishInMenuEntity
{
    public int ID { get; set; }
    public string Name { get; set; }

    public DishInMenuEntity()
    {
        Name = string.Empty;
    }
}
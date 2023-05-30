﻿namespace Restaurant_management_system.Core.DishesAggregate;

public class DishEntity
{
    public int ID { get; set; }
    public int OrderID { get; set; }
    public string DishName { get; set; }
    public DateTime DateOfOrdering { get; set; }
    public bool IsDone { get; set; }
    public bool IsTakenAway { get; set; }
    public bool IsPrioritized { get; set; }

    public DishEntity()
    {
        DishName = string.Empty;
        DateOfOrdering = DateTime.Now;
        IsDone = false;
        IsTakenAway = false;
        IsPrioritized = false;
    }
}
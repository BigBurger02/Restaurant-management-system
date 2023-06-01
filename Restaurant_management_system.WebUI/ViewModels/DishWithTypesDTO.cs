using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class DishWithTypesDTO
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public int OrderID { get; set; }
    public string DishName { get; set; }
    public string TimeOfOrdering { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public bool IsTakenAway { get; set; }
    public bool IsPrioritized { get; set; }
}
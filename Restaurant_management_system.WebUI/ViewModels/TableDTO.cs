using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class TableDTO
{
    public int ID { get; set; }
    public string IsOccupiedString { get; set; } = string.Empty;
    public string IsPaidString { get; set; } = string.Empty;
    public bool IsOccupiedBool { get; set; } = false;
    public bool IsPaidBool { get; set; } = false;
    public int AmountOfGuests { get; set; } = 0;
    public int OrderCost { get; set; } = 0;

    public OrderInTableDTO? Order { get; set; }
}


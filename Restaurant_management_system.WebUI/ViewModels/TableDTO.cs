using System;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.WebUI.ViewModels;

public class TableDTO
{
    public int ID { get; set; }
    public string IsOccupied { get; set; } = string.Empty;
    public string IsPaid { get; set; } = string.Empty;
    public int AmountOfGuests { get; set; } = 0;
    public int OrderCost { get; set; } = 0;
    public OrderEntity? Order { get; set; }
}


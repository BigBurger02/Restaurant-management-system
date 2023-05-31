using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class TableWithTypesDTO
{
    public int ID { get; set; }
    public bool IsOccupied { get; set; } = false;
    public bool IsPaid { get; set; } = false;
    public int AmountOfGuests { get; set; } = 0;
}


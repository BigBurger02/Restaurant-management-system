using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class OrderInTableDTO
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public bool Open { get; set; }
    public string? Message { get; set; } = string.Empty;

    public List<DishInOrderDTO>? Dishes { get; set; } = new List<DishInOrderDTO>();
}
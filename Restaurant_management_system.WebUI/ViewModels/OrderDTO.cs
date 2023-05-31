using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class OrderDTO
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public string? Message { get; set; } = string.Empty;

    public List<DishDTO>? Dishes { get; set; } = new List<DishDTO>();
}
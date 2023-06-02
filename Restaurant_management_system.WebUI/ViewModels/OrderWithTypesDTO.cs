using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class OrderWithTypesDTO
{
    public int ID { get; set; }
    public int TableID { get; set; }
    public string? Message { get; set; } = string.Empty;

    public List<DELETE_DishWithTypesDTO>? Dishes { get; set; } = new List<DELETE_DishWithTypesDTO>();
}


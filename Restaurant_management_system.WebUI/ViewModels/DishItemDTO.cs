using System;

namespace Restaurant_management_system.WebUI.ViewModels;

public class DishItemDTO
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public string IngredientsForCustomer { get; set; } = string.Empty;
}


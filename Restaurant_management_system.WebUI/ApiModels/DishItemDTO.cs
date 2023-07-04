namespace Restaurant_management_system.WebUI.ApiModels;

public class DishItemDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public List<string>? Ingredients { get; set; }

    public DishItemDTO()
    {
        Name = string.Empty;
        Price = 0;

        Ingredients = new List<string>();
    }
}


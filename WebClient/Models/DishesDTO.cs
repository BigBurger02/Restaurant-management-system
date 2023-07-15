namespace WebClient.Models;

public class DishesDTO
{
	public int ID { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Price { get; set; }

	public DishesDTO()
	{
	}
}
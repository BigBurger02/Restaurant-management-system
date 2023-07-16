namespace WebClient.Models;

public class DishesDTO
{
	public int id { get; set; }
	public string name { get; set; } = string.Empty;
	public int price { get; set; }

	public DishesDTO()
	{
	}
}
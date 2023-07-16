using System;
namespace WebClient.Models
{
	public class MenuDTO
	{
		public int id { get; set; }
		public string name { get; set; } = string.Empty;
		public int price { get; set; }

		public List<Ingredient> ingredients { get; set; } = new List<Ingredient>();
	}

	public class Ingredient
	{
		public int id { get; set; }
		public string name { get; set; } = string.Empty;
		public int price { get; set; }
	}
}
using System;
namespace WebClient.Models
{
	public class MenuDTO
	{
		public int id { get; set; }
		public string name { get; set; } = string.Empty;
		public int price { get; set; }

		public List<string> ingredients { get; set; } = new List<string>();

		public MenuDTO()
		{

		}
	}
}


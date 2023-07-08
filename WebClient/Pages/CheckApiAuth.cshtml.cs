using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages
{
	public class CheckApiAuth : PageModel
	{
		public string Json = string.Empty;

		public async Task OnGet()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var client = new HttpClient();

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var content = await client.GetStringAsync("https://localhost:9002/identity");
			//var content = await client.GetStringAsync("https://restaurant-management-system.azurewebsites.net/identity");

			var parsed = JsonDocument.Parse(content);
			var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

			Json = formatted;
		}
	}
}

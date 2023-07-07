using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class MenuController : Controller
	{
		[HttpGet]
		public async Task<IActionResult> GetMenu()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			//client.DefaultRequestHeaders.
			//var content = await client.GetStringAsync("https://localhost:9002/identity");
			var content = await client.GetStringAsync("https://localhost:9002/api/menu");

			var parsed = JsonDocument.Parse(content);
			var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

			ViewData["menu"] = formatted;

			return View();
		}
	}
}


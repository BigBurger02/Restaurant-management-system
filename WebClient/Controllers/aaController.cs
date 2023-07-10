using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
	public class aaController : Controller
	{
		public async Task<IActionResult> checktokenfromclient()
		{
			var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() };
			if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
			{
				return NotFound();
			}

			var userInfo = new UserInfo(await HttpContext.AuthenticateAsync());

			return View(userInfo);
		}

		[HttpGet]
		public async Task<IActionResult> getmenu()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var content = await client.GetStringAsync("https://localhost:9002/api/menu");
			//var content = await client.GetStringAsync("https://restaurant-management-system.azurewebsites.net/api/menu");

			var parsed = JsonDocument.Parse(content);
			var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

			ViewData["menu"] = formatted;

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> checktokenfromapi()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var client = new HttpClient();

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var content = await client.GetStringAsync("https://localhost:9002/identity");
			//var content = await client.GetStringAsync("https://restaurant-management-system.azurewebsites.net/identity");

			var parsed = JsonDocument.Parse(content);
			var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

			ViewData["data"] = formatted;

			return View();
		}

		public async Task<string> mail()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var client = new HttpClient();

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var content = await client.GetStringAsync("https://localhost:9002/api/mail");
			//var content = await client.GetStringAsync("https://restaurant-management-system.azurewebsites.net/api/mail");

			return "sent";
		}
	}
}


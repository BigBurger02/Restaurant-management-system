using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using WebClient.Models;
using System.Text.Json;

namespace WebClient.Controllers;

[Authorize]
public class MenuController : Controller
{
	private readonly IConfigurationSection ConfigSection;
	private readonly string ISUri; // IdentityServer Uri
	private readonly string ApiUri; //Api Uri
	private readonly int TableID;

	public MenuController(IConfiguration configRoot)
	{
		ConfigSection = configRoot.GetSection("ClientSecrets");
		ISUri = "https://" + ConfigSection.GetValue<string>("IdentityServer:uri") + "/";
		ApiUri = "https://" + ConfigSection.GetValue<string>("Api:uri") + "/";
		TableID = 1;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllMenu()
	{
		var accessToken = await HttpContext.GetTokenAsync("access_token");
		var client = new HttpClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		var content = await client.GetStringAsync(ApiUri + "api/menu");

		IEnumerable<MenuDTO> dishes = JsonSerializer.Deserialize<List<MenuDTO>>(content)!;

		var orderID = ReadOrderIDCookie();
		if (orderID != 0)
		{
			ViewData["orderID"] = orderID;
		}

		return View(dishes);
	}

	[HttpGet]
	public async Task<IActionResult> GetCart()
	{
		var orderID = ReadOrderIDCookie();
		if (orderID == 0)
			return View();

		var accessToken = await HttpContext.GetTokenAsync("access_token");
		var client = new HttpClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		var content = await client.GetStringAsync(ApiUri + "api/cart/" + orderID);

		IEnumerable<DishesDTO> dishes = JsonSerializer.Deserialize<List<DishesDTO>>(content)!;

		return View(dishes);
	}

	public async Task<int> CreateOrder()
	{
		var accessToken = await HttpContext.GetTokenAsync("access_token");
		var httpClient = new HttpClient(new HttpClientHandler())
		{
			BaseAddress = new Uri(ApiUri + "api/order/" + TableID)
		};
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		string responseBody;
		using (var response = httpClient.PostAsync(ApiUri + "api/order/" + TableID, null))
		{
			responseBody = response.Result.Content.ReadAsStringAsync().Result;
		}

		if (int.TryParse(responseBody, out int orderID))
		{
			SetOrderIDCookie(orderID);
			return orderID;
		}

		return 0;
	}

	public async Task<IActionResult> AddToCart(int dishID)
	{
		var orderID = ReadOrderIDCookie();
		if (orderID == 0)
		{
			orderID = await CreateOrder();
			if (orderID == 0)
				return StatusCode(500, "Error: after creating order orderID = 0");
		}

		var accessToken = await HttpContext.GetTokenAsync("access_token");
		var client = new HttpClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		var result = await client.PostAsync(ApiUri + "api/cart/" + orderID + "/" + dishID, null);
		if (result.IsSuccessStatusCode)
			return RedirectToAction("GetAllMenu");
		else
			return StatusCode(500, "Response from add to cart endopoint: 500");
	}



	private int ReadOrderIDCookie()
	{
		if (int.TryParse(Request.Cookies["OrderID"], out int orderID))
			return orderID;
		return 0;
	}
	private void SetOrderIDCookie(int orderID)
	{
		var cookieOptions = new CookieOptions();
		cookieOptions.Expires = DateTime.Now.AddMinutes(60);
		cookieOptions.Path = "/";
		Response.Cookies.Append("OrderID", orderID.ToString(), cookieOptions);
	}
	public IActionResult ResetOrderIDCookie()
	{
		var cookieOptions = new CookieOptions();
		cookieOptions.Expires = DateTime.Now.AddSeconds(10);
		cookieOptions.Path = "/";
		Response.Cookies.Append("OrderID", "0", cookieOptions);

		return RedirectToAction("GetAllMenu");
	}
}
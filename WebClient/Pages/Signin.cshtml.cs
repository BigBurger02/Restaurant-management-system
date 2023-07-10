using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Pages;

[Authorize]
public class SigninModel : PageModel
{
	public IActionResult OnGet()
	{
		return RedirectToAction("Index", "Home");
	}
}

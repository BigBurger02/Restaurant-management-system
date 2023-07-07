using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant_management_system.Infrastructure.Data;


namespace Restaurant_management_system.WebUI.Api
{
	[Route("identity")]
	[Authorize]
	public class IdentityController : Controller
	{
		[HttpGet]
		public IActionResult Get()
		{
			return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
			//return new JsonResult(User.IsInRole("Admin"));

			//var userInfo = new UserInfo(await HttpContext.AuthenticateAsync());
			//return new JsonResult(userInfo);
			//return View(userInfo);
		}
	}
}
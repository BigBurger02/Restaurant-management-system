using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
		}
	}
}
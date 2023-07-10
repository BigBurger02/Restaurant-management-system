using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.EntityFrameworkCore;
using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.Services.Attributes;
using Restaurant_management_system.Core.Services.Logger;
using Restaurant_management_system.Core.TablesAggregate;
using Restaurant_management_system.Core.MailAggregate;

namespace Restaurant_management_system.WebUI.Api
{
	[AllowAnonymous]
	[Route("api/[controller]")]
	[SwaggerControllerOrder(4)]
	public class MailController : Controller
	{
		private readonly ILogger<MailController> _logger;
		private readonly IMyEmailSender _emailSender;

		public MailController(ILogger<MailController> logger, IMyEmailSender emailSender)
		{
			_logger = logger;
			_emailSender = emailSender;
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[Produces("application/json")]
		public async Task<IActionResult> SendEmail()
		{
			_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

			List<string> to = new List<string> { "vlad.polovoy@gmail.com" };
			MailData data = new MailData(to, "SubjecT", "BodY");
			await _emailSender.SendAsync(data, CancellationToken.None);

			_logger.LogInformation(9000, "Email sent");

			return Ok("Email sent");
		}
	}
}
/// <summary>
/// Send email to one receiver
/// </summary>
/// <returns></returns>
/// <remarks>
/// Sample request:
/// 
///     PUT api/order/5
/// </remarks>
/// <param name=""></param>
/// <response code="404">Table not found</response>
/// <response code="201">Returns the orderID of created order. Order connected to given tableID</response>
/// <response code="500">Server error</response>
// PUT: api/order/5
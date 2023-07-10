using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;

using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.Services.Attributes;
using Restaurant_management_system.Core.Services.Logger;
using Restaurant_management_system.Core.MailAggregate;
using System.Text.Encodings.Web;

namespace Restaurant_management_system.WebUI.Api;

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

	/// <summary>
	/// Send email to one receiver
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Sample request:
	/// 
	///     GET api/mail?userId=***&code=***
	/// </remarks>
	/// <param name="reciever"></param>
	/// <param name="userID"></param>
	/// <param name="code"></param>
	/// <response code="200">Email sent</response>
	/// <response code="500">Server error</response>
	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(500)]
	[Produces("application/json")]
	public async Task<IActionResult> SendEmail(string reciever, string userID, string code)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "{route} visited at {time} by {user}. LogEvent:{logevent}", ControllerContext.ToCtxString(), DateTime.UtcNow.ToString(), User.Identity!.Name, LogEvents.VisitMethod);

		var callbackUrl =
			"https://localhost:9003/Account/Signup/ConfirmEmail" +
			"?" +
			"userId=" +
			userID +
			"&" +
			"code=" +
			code;

		string body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
		List<string> to = new List<string> { reciever };
		MailData data = new MailData(to, "Confirm email", body);
		await _emailSender.SendAsync(data, CancellationToken.None);

		_logger.LogInformation(9000, "Email sent");

		return Ok("Email sent");
	}
}
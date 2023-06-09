﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using System.Text.Encodings.Web;

using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.Services.Attributes;
using Restaurant_management_system.Core.Services.Logger;
using Restaurant_management_system.Core.MailAggregate;

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
	/// Send confirmation email
	/// </summary>
	/// <remarks>
	/// Sample request:
	/// 
	///     GET api/mail?userId=***&code=***
	/// </remarks>
	/// <param name="reciever"></param>
	/// <param name="userID"></param>
	/// <param name="code"></param>
	/// <response code="200">Email sent</response>
	[HttpPost]
	[ProducesResponseType(200)]
	[Produces("application/json")]
	public async Task<IActionResult> SendConfirmEmail(string reciever, string userID, string code)
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

		_logger.LogInformation("Email sent");

		return Ok("Email sent");
	}
}
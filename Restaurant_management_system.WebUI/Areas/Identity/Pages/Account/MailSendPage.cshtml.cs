using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Restaurant_management_system.WebUI.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class MailSendPageModel : PageModel
	{




		private readonly ILogger<MailSendPageModel> _logger;
		private readonly IEmailSender _emailSender;

		public MailSendPageModel(



			ILogger<MailSendPageModel> logger,
			IEmailSender emailSender)
		{




			_logger = logger;
			_emailSender = emailSender;
		}

		public void OnGet()
		{
			_logger.LogInformation("mail method");
			_emailSender.SendEmailAsync("vlad.polovoy@gmail.com", "Confirm your email",
						$"Please confirm your account by <a href='google.com'>clicking here</a>.");
			_logger.LogInformation("mail method end");
		}
	}
}

using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using Restaurant_management_system.Core.MailAggregate;
using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.Services.Logger;

namespace Restaurant_management_system.Infrastructure;

public class SmtpEmailSender : IMyEmailSender
{
	private readonly ILogger<SmtpEmailSender> _logger;
	private readonly MailSettings _settings;

	public SmtpEmailSender(ILogger<SmtpEmailSender> logger, IOptions<MailSettings> settings)
	{
		_logger = logger;
		_settings = settings.Value;
	}

	public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
	{
		_logger.LogInformation(LogEvents.VisitMethod, "SmtpEmailSender.SendAsync visited at {time} by {user}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), mailData.DisplayName, LogEvents.VisitMethod);

		try
		{
			var mail = new MimeMessage();

			#region Sender / Receiver
			// Sender
			mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
			mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

			// Receiver
			foreach (string mailAddress in mailData.To)
				mail.To.Add(MailboxAddress.Parse(mailAddress));

			// Set Reply to if specified in mail data
			if (!string.IsNullOrEmpty(mailData.ReplyTo))
				mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

			// BCC
			// Check if a BCC was supplied in the request
			if (mailData.Bcc != null)
			{
				// Get only addresses where value is not null or with whitespace. x = value of address
				foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
					mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
			}

			// CC
			// Check if a CC address was supplied in the request
			if (mailData.Cc != null)
			{
				foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
					mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
			}

			_logger.LogInformation(LogEvents.FormMail, "SmtpEmailSender.SendAsync: Sender / Receiver formed at {time} by {user}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), mailData.DisplayName, LogEvents.FormMail);
			#endregion

			#region Content

			// Add Content to Mime Message
			var body = new BodyBuilder();
			mail.Subject = mailData.Subject;
			body.HtmlBody = mailData.Body;
			mail.Body = body.ToMessageBody();

			_logger.LogInformation(LogEvents.FormMail, "SmtpEmailSender.SendAsync: Content formed at {time} by {user}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), mailData.DisplayName, LogEvents.FormMail);

			#endregion

			#region Send Mail

			using var smtp = new SmtpClient();

			if (_settings.UseSSL)
			{
				await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
			}
			else if (_settings.UseStartTls)
			{
				await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
			}
			await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
			await smtp.SendAsync(mail, ct);
			await smtp.DisconnectAsync(true, ct);

			_logger.LogInformation(LogEvents.SendMail, "SmtpEmailSender.SendAsync: mail sent at {time} by {user}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), mailData.DisplayName, LogEvents.SendMail);

			#endregion

			return true;
		}
		catch (Exception)
		{
			_logger.LogError(LogEvents.MailException, "SmtpEmailSender.SendAsync: Thrown exception at {time} by {user}. LogEvent:{logevent}", DateTime.UtcNow.ToString(), mailData.DisplayName, LogEvents.MailException);

			return false;
		}
	}
}


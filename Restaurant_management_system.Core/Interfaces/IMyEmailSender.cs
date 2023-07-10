using Restaurant_management_system.Core.MailAggregate;

namespace Restaurant_management_system.Core.Interfaces;

public interface IMyEmailSender
{
	Task<bool> SendAsync(MailData mailData, CancellationToken ct);
}


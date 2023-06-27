using Restaurant_management_system.Core.MailAggregate;

namespace Restaurant_management_system.Core.Interfaces;

public interface IEmailSender
{
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);
}


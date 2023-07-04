namespace Restaurant_management_system.Core.Services.Logger;

/// <summary>
/// 1-WIde;
/// 2-Get;
/// 3-Set;
/// 4-Create;
/// 5-Delete;
/// 6-NotFound;
/// 7-Exception;
/// </summary>
public class LogEvents
{
	public const int VisitMethod = 1000;
	public const int LeaveMethod = 1001;
	public const int SendMail = 1002;

	public const int SetDataInDB = 3000;
	public const int FormMail = 3001;

	public const int CreateInDB = 4000;

	public const int RemoveFromDB = 5000;

	public const int NotFoundInDB = 6000;

	public const int MailException = 7000;
}


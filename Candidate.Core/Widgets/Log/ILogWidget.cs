namespace Candidate.Core.Widgets.Log;

public interface ILogWidget
{
    Task UserLog(int userID, string action, string metaData);

    Task ReportLog(int userID, int reportCode, string status, string input, DateTime startDateTime);

    Task EntityLog(string entityName, int entityID, string action, int userID, string changedFields);

    Task ExceptionLog(Exception exception, string? source, int userID = 0);

    string GetExceptionMessage(Exception ex);
    string GetExceptionHResult(Exception ex);
}
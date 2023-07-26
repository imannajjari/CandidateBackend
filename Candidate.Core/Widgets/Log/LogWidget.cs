using System.Diagnostics;
using System.Reflection;
using Candidate.Core.Widgets.Calendar;
using Candidate.Core.Widgets.Config;
using Candidate.Core.Widgets.Dapper;
using Candidate.Core.Widgets.Method;
using Candidate.Core.Widgets.Stream;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace Candidate.Core.Widgets.Log;

public class LogWidget : ILogWidget, IDisposable
{
    private readonly StreamWidget _stream;
    private readonly string _path;
    private readonly object _lock;
    private bool _disposed;
    private readonly IHttpContextAccessor _accessor;
    private readonly IDapperWidget _dapper;
    private readonly LogType _logType;

    public LogWidget(IHttpContextAccessor accessor,  IDapperWidget dapper)
    {
        var filePath = ConfigWidget.GetConfigValue<string>("FilePathes:LogFilePath");
        _accessor = accessor;
        _path = Path.Combine(filePath, "logs");
        _stream = new StreamWidget(_path);
        _lock = new object();
        _disposed = false;
        _dapper = dapper;
        _logType = ConfigWidget.GetConfigValue<LogType>("LogConfiguration:LogType");
    }


    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================

    /// <summary>
    /// لاگ اتفاقات کاربر
    /// </summary>
    /// <param name="documentID"></param>
    /// <param name="action"></param>
    /// <param name="ip"></param>
    /// <param name="actorID"></param>
    /// <param name="logType"></param>
    public async Task UserLog(int userID, string action, string metaData)
    {
        try
        {
            List<Task> tasks = new List<Task>();
            
             if (_logType == LogType.DataBase)
            {
                tasks.Add(UserLogInDatabase(userID, action, metaData));

            }
            else if (_logType == LogType.File)
            {
                UserLogInFile(userID, action, metaData);
            }
            else if (_logType == LogType.All)
            {
                tasks.Add(UserLogInDatabase(userID, action, metaData));
                UserLogInFile(userID, action, metaData);
            }
            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {
            UserLogInFile(userID, action, metaData);
            ExceptionLogInFile(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 0);
            Logger(MethodBase.GetCurrentMethod()?.GetSourceName());
        }
    }

    private void Logger(string source)
    {
        var line =
            $"Candidate-{source}-J{CalenderWidget.ToJalaliDateTime(DateTime.Now)}-{DateTime.Now.ToString("u")}";

        lock (_lock)
        {
            if (!_disposed)
                _stream.Append(DateTime.Now, "logger", line);
        }
    }


    private async Task UserLogInDatabase(int userID, string action, string metaData)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@System", "Candidate");
        parameters.Add("@UserID", userID);
        parameters.Add("@Action", action);
        parameters.Add("@MetaData", metaData);
        parameters.Add("@EventDateTime", CalenderWidget.ToJalaliDateTime(DateTime.Now));
        parameters.Add("@CreationDateTime", DateTime.Now);
        await _dapper.CallProcedureAsync<string>("sp_userLog_Insert", parameters);
    }

    private void UserLogInFile(int userID, string action, string metaData)
    {
        var line =
               $"Candidate-{userID}-{action}-{metaData}-J{CalenderWidget.ToJalaliDateTime(DateTime.Now)}-{DateTime.Now.ToString("u")}";

        lock (_lock)
        {
            if (!_disposed)
                _stream.Append(DateTime.Now, "users", line);
        }
    }

   


    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================

    /// <summary>
    /// لاگ گزارش ها
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="reportCode"></param>
    /// <param name="status"></param>
    /// <param name="input"></param>
    /// <param name="startDateTime"></param>
    /// <param name="logType"></param>
    /// <returns></returns>
    public async Task ReportLog(int userID, int reportCode, string status, string input, DateTime startDateTime)
    {
        string serial = GenerateSerial();
        var id = Guid.NewGuid();
        DateTime delivery = DateTime.Now;
        var duration = (delivery - startDateTime).Seconds;
        try
        {
            List<Task> tasks = new List<Task>();
            if (_logType == LogType.DataBase)
            {
                tasks.Add(ReportLogInDatabase(id, userID, reportCode, status, input, serial, startDateTime, delivery, duration));
            }
            else if (_logType == LogType.File)
            {
                ReportLogInFile(id, userID, reportCode, status, input, serial, startDateTime, delivery, duration);
            }
            else if (_logType == LogType.All)
            {
                tasks.Add(ReportLogInDatabase(id, userID, reportCode, status, input, serial, startDateTime, delivery, duration));
                ReportLogInFile(id, userID, reportCode, status, input, serial, startDateTime, delivery, duration);
            }
            tasks.Add(UserLog(userID, "REP_GENERATED", id.ToString()));
            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {
            ReportLogInFile(id, userID, reportCode, status, input, serial, startDateTime, delivery, duration);
            ExceptionLogInFile(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 0);
            Logger(MethodBase.GetCurrentMethod()?.GetSourceName());
        }
    }

    private async Task ReportLogInDatabase(Guid id, int userID, int reportCode, string status, string input, string serial, DateTime startDateTime, DateTime deliveryDateTime, int duration)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ID", id);
        parameters.Add("@System", "Candidate");
        parameters.Add("@UserID", userID);
        parameters.Add("@ReportCode", reportCode);
        parameters.Add("@Status", status);
        parameters.Add("@Serial", serial);
        parameters.Add("@Input", input);
        parameters.Add("@StartDateTime", CalenderWidget.ToJalaliDateTime(startDateTime));
        parameters.Add("@DeliveryDateTime", CalenderWidget.ToJalaliDateTime(deliveryDateTime));
        parameters.Add("@Duration", duration);
        parameters.Add("@CreationDateTime", DateTime.Now);
        await _dapper.CallProcedureAsync<int>("sp_reportLog_Insert", parameters);
    }

    private void ReportLogInFile(Guid id, int userID, int reportCode, string status, string input, string serial, DateTime startDateTime, DateTime deliveryDateTime, int duration)
    {
        var line =
                $"Candidate-{id} | {userID}-{reportCode}-{status}-{serial}-{input}-{status}-J{CalenderWidget.ToJalaliDateTime(startDateTime)}-J{CalenderWidget.ToJalaliDateTime(deliveryDateTime)}-{duration}-{DateTime.Now.ToString("u")}";

        lock (_lock)
        {
            if (!_disposed)
                _stream.Append(DateTime.Now, "reports", line);
        }
    }

    

    private string GenerateSerial()
    {
        return "0";
    }


    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================


    /// <summary>
    /// لاگ اتفاقات موجودیت های بیزینسی
    /// for meta data requires to CDC in Database
    /// </summary>
    /// <param name="documentID"></param>
    /// <param name="action"></param>
    /// <param name="ip"></param>
    /// <param name="userID"></param>
    /// <param name="logType"></param>
    public async Task EntityLog(string entityName, int entityID, string action, int userID, string changedFields)
    {
        try
        {
            List<Task> tasks = new List<Task>();
            if (_logType == LogType.DataBase)
            {
                tasks.Add(EntityLogInDatabase(entityName, entityID, action, userID, changedFields));
            }
            else if (_logType == LogType.File)
            {
                EntityLogInFile(entityName, entityID, action, userID, changedFields);
            }
            else if (_logType == LogType.All)
            {
               
                tasks.Add(EntityLogInDatabase(entityName, entityID, action, userID, changedFields));
                EntityLogInFile(entityName, entityID, action, userID, changedFields);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            EntityLogInFile(entityName, entityID, action, userID, changedFields);
            ExceptionLogInFile(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 0);
            Logger(MethodBase.GetCurrentMethod()?.GetSourceName());
        }
    }


    private async Task EntityLogInDatabase(string entityName, int entityID, string action, int userID, string changedFields)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@System", "Candidate");
        parameters.Add("@EntityName", entityName);
        parameters.Add("@EntityID", entityID);
        parameters.Add("@Action", action);
        parameters.Add("@UserID", userID);
        parameters.Add("@EventDateTime", CalenderWidget.ToJalaliDateTime(DateTime.Now));
        parameters.Add("@CreationDateTime", DateTime.Now);
        parameters.Add("@ChangedFields", changedFields);
        await _dapper.CallProcedureAsync<string>("sp_entityLog_Insert", parameters);
    }

    private void EntityLogInFile(string entityName, int entityID, string action, int userID, string changedFields)
    {
        var line = $"Candidate-{entityName}-{entityID}-{action}-{userID}-J{CalenderWidget.ToJalaliDateTime(DateTime.Now)}-{DateTime.Now.ToString("u")}-{changedFields}";

        lock (_lock)
        {
            if (!_disposed)
                _stream.Append(DateTime.Now, "entities", line);
        }
    }

    

    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================
    // ===================================================================================================================

    /// <summary>
    /// لاگ استثنائات سیستمی
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="source"></param>
    /// <param name="userID"></param>
    /// <param name="ip"></param>
    /// <param name="logType"></param>
    public async Task ExceptionLog(Exception exception, string? source, int userID = 0)
    {
        List<Task> tasks = new List<Task>();
        var info = GetExceptionInformations(exception);
        try
        {


            if (_logType == LogType.DataBase)
            {
                tasks.Add(ExceptionLogInDatabase(info[0], info[1], info[2], source ?? info[3], userID));
            }
            else if (_logType == LogType.File)
            {
                ExceptionLogInFile(info[0], info[1], info[2], source ?? info[3], userID);
            }
            else if (_logType == LogType.All)
            {
               
                tasks.Add(ExceptionLogInDatabase(info[0], info[1], info[2], source ?? info[3], userID));
                ExceptionLogInFile(info[0], info[1], info[2], source ?? info[3], userID);
            }
            await Task.WhenAll(tasks);


        }
        catch (Exception ex)
        {
            ExceptionLogInFile(info[0], info[1], info[2], source ?? info[3], userID);
            //await ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 0, logType: LogType.File);
            Logger(MethodBase.GetCurrentMethod()?.GetSourceName());
        }
    }


    private async Task ExceptionLogInDatabase(string message, string innerMessage, string code, string? source, int userID)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@System", "Candidate");
        parameters.Add("@Code", code);
        parameters.Add("@Message", message);
        parameters.Add("@InnerMessage", innerMessage);
        parameters.Add("@Source", source);
        parameters.Add("@UserID", userID);
        parameters.Add("@ExceptionDateTime", CalenderWidget.ToJalaliDateTime(DateTime.Now));
        parameters.Add("@CreationDateTime", DateTime.Now);
        await _dapper.CallProcedureAsync<string>("sp_exceptionLog_Insert", parameters);
    }

    private void ExceptionLogInFile(string message, string innerMessage, string code, string? source, int userID)
    {
        var line = $"Candidate-{message}-{innerMessage}-{code}-{source}-{userID}-J{DateTime.Now.ToJalaliDateTime()}-{DateTime.Now.ToString("u")}";

        lock (_lock)
        {
            if (!_disposed)
                _stream.Append(DateTime.Now, "exceptions", line);
        }
    }

    private void ExceptionLogInFile(Exception exception, string? source, int userID = 0)
    {
        try
        {
            var info = GetExceptionInformations(exception);
            ExceptionLogInFile(info[0], info[1], info[2], source ?? info[3], userID);
        }
        catch (Exception ex)
        {
            Logger(MethodBase.GetCurrentMethod()?.GetSourceName());
        }
    }

    


    private List<string> GetExceptionInformations(Exception exception)
    {
        List<string> result = new List<string>();
        var st = new StackTrace(exception, true);
        // Get the top stack frame
        var frame = st.GetFrame(0);
        // دریافت شماره خط خطا
        var lineNumber = frame?.GetFileLineNumber() ?? 0;
        var messages = FromHierarchy(exception, ex => ex.InnerException).Select(ex => ex.Message);
        var innerMessages = string.Join("////", messages);
        var stackTractObject = new StackTrace();
        var reflectedType = stackTractObject.GetFrame(1)?.GetMethod()?.ReflectedType;
        string source = string.Empty;
        if (reflectedType != null)
        {
            source = reflectedType.Name;
        }
        result.Add($"{exception.Message} In Line {lineNumber}");
        result.Add(innerMessages);
        result.Add(exception.HResult.ToString());
        result.Add(source);
        return result;
    }





    /// <summary>
    /// دریافت پیام ناشی از استثنای پیشامد
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public string GetExceptionMessage(Exception ex)
    {
#if DEBUG
        var result = ex.Message;
        return result;
#else
            var result = "استثنای ناشناخته";
            return result;
# endif
    }
    /// <summary>
    /// دریافت پیام ناشی از استثنای پیشامد
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public string GetExceptionHResult(Exception ex)
    {
#if DEBUG
        var result = ex.HResult.ToString();
        return result;
#else
            var result = "کد خطای ناشناخته";
            return result;
# endif
    }
    #region private Methods
    public IEnumerable<TSource> FromHierarchy<TSource>(TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
    {
        for (var current = source; canContinue(current); current = nextItem(current))
        {
            yield return current;
        }
    }

    public IEnumerable<TSource> FromHierarchy<TSource>(TSource source, Func<TSource, TSource> nextItem) where TSource : class
    {
        return FromHierarchy(source, nextItem, s => s != null);
    }


    #endregion
    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
                return;

            if (_stream != null)
                _stream.Dispose();

            _disposed = true;
        }
    }
}
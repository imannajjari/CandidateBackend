using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Core.Resources;
using Candidate.Core.Widgets.DataParser.Interface;
using Candidate.Core.Widgets.Log;
using Candidate.Data.Models;
using CsvHelper;

namespace Candidate.Core.Widgets.DataParser;

public class CsvParser : IDataParser
{
    private readonly ILogWidget _log;

    public CsvParser(ILogWidget log)
    {
        _log = log;
    }
    public ResultViewModel<Person> Parse(InputViewModel data)
    {
        var result = new ResultViewModel<Person>();
        var errors = new List<ErrorViewModel>();
        try
        {
            using var reader = new StringReader(data.Data);
            using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));
            result.Result = csv.GetRecords<Person>().FirstOrDefault();
            // انجام عملیات با داده‌های csvData
            result.Message = new MessageViewModel()
            {
                ID = 0,
                Status = Statuses.Success,
                Title = Titles.Parse,
                Message = Messages.ParsedSuccesses,
                Errors = errors,
                Value = ""
            };
        }
        catch (Exception ex)
        {
            errors.Add(new ErrorViewModel()
            {
                ErrorCode = ex.HResult.ToString(),
                ErrorMessage = _log.GetExceptionMessage(ex)
            });
            errors.Add(new ErrorViewModel()
            {
                ErrorCode = "104",
                ErrorMessage = Messages.InvalidCsv
            });
            result.Message = new MessageViewModel()
            {
                ID = -1,
                Status = Statuses.Error,
                Title = Titles.Exception,
                Message = Messages.ParseFaild,
                Errors = errors,
                Value = ""
            };
        }
        return result;
    }
}
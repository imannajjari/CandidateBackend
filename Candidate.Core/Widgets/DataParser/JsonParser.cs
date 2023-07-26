using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Core.Resources;
using Candidate.Core.Widgets.DataParser.Interface;
using Candidate.Core.Widgets.Log;
using Candidate.Data.Models;
using Castle.Components.DictionaryAdapter;

namespace Candidate.Core.Widgets.DataParser;

public class JsonParser : IDataParser
{
    private readonly ILogWidget _log;

    public JsonParser(ILogWidget log)
    {
        _log = log;
    }

    public ResultViewModel<Person> Parse(InputViewModel data)
    {
        var result = new ResultViewModel<Person>();
        var errors = new List<ErrorViewModel>();
        try
        {
            result.Result = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(data.Data);
            // انجام عملیات با داده‌های jsonData
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
                ErrorCode = "102",
                ErrorMessage = Messages.InvalidJSON
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
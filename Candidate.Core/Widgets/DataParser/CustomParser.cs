using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Core.Resources;
using Candidate.Core.Widgets.Convertor;
using Candidate.Core.Widgets.DataParser.Interface;
using Candidate.Data.Models;

namespace Candidate.Core.Widgets.DataParser;

public class CustomParser : IDataParser
{
    
    public ResultViewModel<Person> Parse(InputViewModel data)
    {
        var result = new ResultViewModel<Person>();
        var errors = new List<ErrorViewModel>();
        try
        {
            var splitData = data.Data.Split('/');
            Person person = new Person
            {
                FirstName = splitData[0],
                LastName = splitData[1],
                BasicSalary = splitData[2].ToLong(),
                Allowance = splitData[3].ToLong(),
                Transportation = splitData[4].ToLong(),
                Date = splitData[5],
                HoursWorked = splitData[6].ToInt()
            };
            result.Result = person;
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
                ErrorMessage = Messages.UnknownException
            });
            errors.Add(new ErrorViewModel()
            {
                ErrorCode = "104",
                ErrorMessage = Messages.InvalidCustom
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
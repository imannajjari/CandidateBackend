using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Core.Resources;
using Candidate.Core.Widgets.DataParser.Interface;
using Candidate.Data.Models;
using System.Xml.Serialization;


namespace Candidate.Core.Widgets.DataParser;

public class XmlParser : IDataParser
{
    

    public ResultViewModel<Person> Parse(InputViewModel data)
    {
        var result = new ResultViewModel<Person>();
        var errors = new List<ErrorViewModel>();
        try
        {
            var serializer = new XmlSerializer(typeof(Person));
            using (var reader = new StringReader(data.Data))
            {
                result.Result = (Person)serializer.Deserialize(reader);
                // انجام عملیات با داده‌های xmlData
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
                ErrorCode = "103",
                ErrorMessage = Messages.InvalidXML
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
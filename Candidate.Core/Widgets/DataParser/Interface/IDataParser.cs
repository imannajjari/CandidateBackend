using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Data.Models;

namespace Candidate.Core.Widgets.DataParser.Interface;


public interface IDataParser
{
    ResultViewModel<Person> Parse( InputViewModel data);
}

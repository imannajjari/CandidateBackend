using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Data.Interfaces;
using Candidate.Data.Models;

namespace Candidate.Core.Services.Interfaces;

public interface IPersonService:IRepository<Person>
{
    MessageViewModel Add(Person entity, string overTimeCalculator);
    MessageViewModel Edit(Person entity, string overTimeCalculator);
    MessageViewModel Remove(int id, bool hardDelete = false);

    ResultViewModel<PersonViewModel> Get(string personCode, string date);

    ResultViewModel<PersonViewModel> GetRange(string personCode, string startDate, string endDate);
    ResultViewModel<Person> ParseData(string datatype, InputViewModel data);
}
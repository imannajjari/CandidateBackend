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

    ResultViewModel<PersonViewModel> Get(int personCode, string date);

    ResultViewModel<PersonViewModel> GetRange(int personCode, int startDate, int endDate);
    ResultViewModel<Person> ParseData(string datatype, InputViewModel data);
}
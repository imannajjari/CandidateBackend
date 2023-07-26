using Candidate.Core.Presentations.Base;
using Candidate.Data.Interfaces;
using Candidate.Data.Models;

namespace Candidate.Core.Services.Interfaces;

public interface IPersonService:IRepository<Person>
{
    MessageViewModel Add(Person entity, string overTimeCalculator);
    MessageViewModel Edit(Person entity, string overTimeCalculator);
    MessageViewModel Remove(int id, bool hardDelete = false);
    
    Person Get(int personCode,string date);

    List<Person> GetRange(int personCode,int startDate, int endDate);
}
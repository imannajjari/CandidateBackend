using Candidate.Data.Models;

namespace Candidate.Core.Presentations.Persons;

public class PersonViewModel
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonCode { get; set; }
    public long BasicSalary { get; set; }
    public long Allowance { get; set; }
    public long Transportation { get; set; }
    public int hoursWorked { get; set; }
    public double TotalSalary { get; set; }
    public string Date { get; set; }
    public bool IsActive { get; set; }
}

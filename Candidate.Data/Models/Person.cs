using Candidate.Data.Interfaces;

namespace Candidate.Data.Models;

public class Person:IEntity
{
   
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonCode  { get; set; }
    public long BasicSalary { get; set; }
    public long Allowance { get; set; }
    public long Transportation { get; set; }
    public int hoursWorked { get; set; }
    public double TotalSalary { get; set; }
    public string Date { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime? ModificationDateTime { get; set; }

}
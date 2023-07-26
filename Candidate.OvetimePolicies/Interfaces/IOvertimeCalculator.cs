namespace Candidate.OvertimePolicies.Interfaces;

public interface IOvertimeCalculator
{
    double CalculateOvertime( long basicSalary, long allowance,int hoursWorked);
}
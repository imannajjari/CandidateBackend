using Candidate.OvertimePolicies.Interfaces;

namespace Candidate.OvertimePolicies.Services;

public class CalculatorA : IOvertimeCalculator
{
    public double CalculateOvertime(long basicSalary, long allowance, int hoursWorked)
    {
        double overtimeRate = 1.5;
        if (hoursWorked > 50)
            overtimeRate = 2.0;

        return (basicSalary + allowance)/196 * (hoursWorked * overtimeRate);
    }
}

public class CalculatorB : IOvertimeCalculator
{
    public double CalculateOvertime(long basicSalary, long allowance, int hoursWorked)
    {
        double overtimeRate = 1.5;
        if (hoursWorked > 40)
            overtimeRate = 2.0;

        return (basicSalary+allowance) / 196 * (hoursWorked * overtimeRate);
    }
}

public class CalculatorC : IOvertimeCalculator
{
    public double CalculateOvertime(long basicSalary, long allowance, int hoursWorked)
    {
        double overtimeRate = 1.5;
        return (basicSalary + allowance) / 196 * (hoursWorked * overtimeRate);
    }
}

namespace Candidate.OvertimePolicies.Interfaces;

public interface IOvertimeCalculatorFactory
{
    IOvertimeCalculator CreateOvertimeCalculator(string type);
}
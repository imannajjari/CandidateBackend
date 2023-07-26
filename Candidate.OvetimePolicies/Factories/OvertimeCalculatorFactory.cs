using Candidate.OvertimePolicies.Interfaces;
using Candidate.OvertimePolicies.Services;

namespace Candidate.OvertimePolicies.Factories;

public class OvertimeCalculatorFactory : IOvertimeCalculatorFactory
{
    public IOvertimeCalculator CreateOvertimeCalculator(string type)
    {
        switch (type.ToLower())
        {
            case "CalculatorC":
                return new CalculatorC();
            case "CalculatorB":
                return new CalculatorB();
            case "CalculatorA":
                return new CalculatorA();
            default:
                throw new ArgumentException("Invalid calculator type.");
        }
    }
}
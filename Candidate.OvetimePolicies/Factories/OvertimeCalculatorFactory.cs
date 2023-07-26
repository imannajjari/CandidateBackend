using Candidate.OvertimePolicies.Interfaces;
using Candidate.OvertimePolicies.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Candidate.OvertimePolicies.Factories;

public class OvertimeCalculatorFactory : IOvertimeCalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public OvertimeCalculatorFactory(IServiceProvider serviceProvider)
    {
           _serviceProvider = serviceProvider;
    }

    public IOvertimeCalculator CreateOvertimeCalculator(string type)
    {
        IOvertimeCalculator calculator;
        switch (type.ToLower())
        {
            case "calculatorc":
                calculator = _serviceProvider.GetService<CalculatorC>();
                return calculator;
            case "calculatorb":
                calculator = _serviceProvider.GetService<CalculatorB>();
                return calculator;
            case "calculatora":
                calculator =_serviceProvider.GetService<CalculatorA>();
                return calculator;
            default:
                throw new ArgumentException("Invalid calculator type.");
                
        }
    }
}
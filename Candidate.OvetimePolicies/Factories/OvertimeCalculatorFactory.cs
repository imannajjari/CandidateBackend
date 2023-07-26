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
        switch (type.ToLower())
        {
            case "CalculatorC":
                return _serviceProvider.GetService<CalculatorC>();
            case "CalculatorB":
                return  _serviceProvider.GetService<CalculatorB>();
            case "CalculatorA":
                return _serviceProvider.GetService<CalculatorA>();
            default:
                throw new ArgumentException("Invalid calculator type.");
        }
    }
}
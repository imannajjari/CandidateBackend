using Candidate.OvertimePolicies.Factories;
using Candidate.OvertimePolicies.Interfaces;

namespace Candidate.Api.IoC;

public static class Injector
{
    public static void RegisterServices(IServiceCollection service)
    {
        service.AddScoped<IOvertimeCalculatorFactory, OvertimeCalculatorFactory>();
    }
}
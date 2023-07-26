using Candidate.Core.Services;
using Candidate.Core.Services.Interfaces;
using Candidate.Core.Widgets.DataParser;
using Candidate.Core.Widgets.DataParser.Interface;
using Candidate.OvertimePolicies.Factories;
using Candidate.OvertimePolicies.Interfaces;
using Candidate.OvertimePolicies.Services;

namespace Candidate.Api.IoC;

public static class Injector
{
    public static void RegisterServices(IServiceCollection service)
    {
        service.AddScoped<IPersonService, PersonService>();

        service.AddTransient<IOvertimeCalculator, CalculatorA>();
        service.AddTransient<IOvertimeCalculator, CalculatorB>();
        service.AddTransient<IOvertimeCalculator, CalculatorC>();
        service.AddTransient<IOvertimeCalculatorFactory, OvertimeCalculatorFactory>();
       
     

        service.AddTransient<IDataParser, JsonParser>();
        service.AddTransient<IDataParser, XmlParser>();
        service.AddTransient<IDataParser, CsvParser>();
        service.AddTransient<IDataParser, CustomParser>();
        service.AddTransient<IDataParserFactory, DataParserFactory>();
    }
}
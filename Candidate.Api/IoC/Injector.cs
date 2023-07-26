using Candidate.Core.Services;
using Candidate.Core.Services.Interfaces;
using Candidate.Core.Widgets.Dapper;
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


        service.AddScoped<CalculatorA>()
            .AddScoped<IOvertimeCalculator, CalculatorA>(s => s.GetService<CalculatorA>());
        service.AddScoped<CalculatorB>()
            .AddScoped<IOvertimeCalculator, CalculatorB>(s => s.GetService<CalculatorB>());
        service.AddScoped<CalculatorC>()
            .AddScoped<IOvertimeCalculator, CalculatorC>(s => s.GetService<CalculatorC>());
        service.AddScoped<IOvertimeCalculatorFactory, OvertimeCalculatorFactory>();


        service.AddScoped<JsonParser>()
            .AddScoped<IDataParser, JsonParser>(s => s.GetService<JsonParser>());
        service.AddScoped<XmlParser>()
            .AddScoped<IDataParser, XmlParser>(s => s.GetService<XmlParser>());
        service.AddScoped<CsvParser>()
            .AddScoped<IDataParser, CsvParser>(s => s.GetService<CsvParser>());
        service.AddScoped<CustomParser>()
            .AddScoped<IDataParser, CustomParser>(s => s.GetService<CustomParser>());
        service.AddTransient<IDataParserFactory, DataParserFactory>();

        service.AddScoped<IDapperWidget, DapperWidget>();
    }
}
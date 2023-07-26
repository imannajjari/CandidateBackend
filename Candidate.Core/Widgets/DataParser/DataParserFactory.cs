using Candidate.Core.Widgets.DataParser.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Candidate.Core.Widgets.DataParser;

public class DataParserFactory: IDataParserFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DataParserFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDataParser CreateParser(string datatype)
    {
        switch (datatype.ToLower())
        {
            case "json":
                return _serviceProvider.GetService<JsonParser>();// _serviceProvider.GetService<JsonParser>();
            case "xml":
                return _serviceProvider.GetService<XmlParser>();
            case "csv":
                return _serviceProvider.GetService<CsvParser>();
            case "custom":
                return _serviceProvider.GetService<CustomParser>();
            default:
                throw new ArgumentException("Invalid datatype.");
        }
    }

}
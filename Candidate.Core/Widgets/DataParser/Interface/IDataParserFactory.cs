namespace Candidate.Core.Widgets.DataParser.Interface;

public interface IDataParserFactory
{
    IDataParser CreateParser(string datatype);
}
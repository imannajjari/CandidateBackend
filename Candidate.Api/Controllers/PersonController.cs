using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Core.Services.Interfaces;
using Candidate.Core.Widgets.DataParser.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Api.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly IDataParserFactory _dataParserFactory;

    public PersonController(IPersonService personService, IDataParserFactory dataParserFactory)
    {
        _personService = personService;
        _dataParserFactory = dataParserFactory;
    }

    public MessageViewModel Add([FromRoute] string datatype, [FromBody] InputViewModel data)
    {
        var factory = _dataParserFactory.CreateParser(datatype);
        var personData = factory.Parse(data);
        var result = _personService.Add(personData.Result, data.OverTimeCalculator);
        return result;
    }
}
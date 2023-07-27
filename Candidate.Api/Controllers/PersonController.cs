using Candidate.Core.Presentations.Base;
using Candidate.Core.Presentations.Persons;
using Candidate.Core.Services.Interfaces;
using Candidate.Core.Widgets.DataParser.Interface;
using Candidate.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Api.Controllers;
[ApiController]
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly IDataParserFactory _dataParserFactory;

    public PersonController(IPersonService personService, IDataParserFactory dataParserFactory)
    {
        _personService = personService;
        _dataParserFactory = dataParserFactory;
    }
    [HttpPost]
    [Route("{datatype=json}/Person/add")]
    public MessageViewModel Add([FromRoute] string datatype, [FromBody] InputViewModel data)
    {
        var factory = _dataParserFactory.CreateParser(datatype);
        var personData = factory.Parse(data);
        var result = _personService.Add(personData.Result, data.OverTimeCalculator);
        return result;
    }

    [HttpPost]
    [Route("{datatype=json}/Person/edit")]
    public MessageViewModel Edit([FromRoute] string datatype, [FromBody] InputViewModel data)
    {
        var factory = _dataParserFactory.CreateParser(datatype);
        var personData = factory.Parse(data);
        var result = _personService.Edit(personData.Result, data.OverTimeCalculator);
        return result;
    }

    [HttpGet]
    [Route("Person/remove/{personID}")]
    public MessageViewModel Remove(int personID)
    {
        var result = _personService.Remove(personID);
        return result;
    }
    [HttpGet]
    [Route("Person/get/{personCode}/{date}")]
    public ResultViewModel<PersonViewModel> Get(string personCode, string date)
    {
        var result = _personService.Get(personCode, date);
        return result;
    }

    [HttpGet]
    [Route("Person/getRange/{personCode}/{startDate}/{endDate}")]
    public ResultViewModel<PersonViewModel> GetRange(string personCode, string startDate, string endDate)
    {
        var result = _personService.GetRange(personCode, startDate, endDate);
        return result;
    }
}
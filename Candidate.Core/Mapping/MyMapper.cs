using AutoMapper;
using Candidate.Core.Presentations.Persons;
using Candidate.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Candidate.Core.Mapping;

public class MyMapper : Profile
{
    public MyMapper()
    {
        CreateMap<Person, PersonViewModel>();
    }
}
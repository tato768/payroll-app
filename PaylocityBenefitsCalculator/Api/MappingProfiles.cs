using AutoMapper;
using Api.Models;
using Api.Dtos.Employee;
using Api.Dtos.Dependent;

namespace Api;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Employee, GetEmployeeDto>();
        CreateMap<Dependent, GetDependentDto>();
    }
}

using AutoMapper;
using Api.Models;
using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using Api.Dtos.Paycheck;

namespace Api;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Employee, GetEmployeeDto>();
        CreateMap<Dependent, GetDependentDto>();
        CreateMap<Paycheck, GetPaycheckDto>();
        CreateMap<BenefitCostExplain, BenefitCostExplainDto>();
    }
}

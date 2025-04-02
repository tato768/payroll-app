using AutoMapper;
using Api.Dtos.Paycheck;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Repositories;
using Api.Calculator;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly IPaycheckCalculator paycheckCalculator;
    private readonly IMapper mapper;

    public EmployeesController(
        IEmployeeRepository employeeRepository,
        IPaycheckCalculator paycheckCalculator,
        IMapper mapper)
    {
        this.employeeRepository = employeeRepository;
        this.paycheckCalculator = paycheckCalculator;
        this.mapper = mapper;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        // there could be some common infra to return one dto (or a list of them)
        // it could do the transformation in generic way and also return 404 if null
        var employee = await employeeRepository.GetById(id);

        if (employee == null)
        {
            return NotFound();
        }

        var employeeDto = mapper.Map<GetEmployeeDto>(employee);
        return new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await employeeRepository.GetAll();
        var employeesDto = mapper.Map<List<GetEmployeeDto>>(employees);

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeesDto,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get paycheck for employee")]
    [HttpGet("{employeeId}/paycheck/{year}/{paycheckNumber}")] //TODO: this path could be nicer
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int employeeId, int year, int paycheckNumber)
    {
        var employee = await employeeRepository.GetById(employeeId);
        if (employee == null)
        {
            return NotFound();
        }

        //TODO: validate year and paycheckNumber
        var paycheck = paycheckCalculator.GetPaycheck(employee, year, paycheckNumber);

        var paycheckDto = mapper.Map<GetPaycheckDto>(paycheck);
        return new ApiResponse<GetPaycheckDto>
        {
            Data = paycheckDto,
            Success = true
        };
    }

}

using AutoMapper;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Repositories;


namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentRepository dependentRepository;
    private readonly IMapper mapper;

    public DependentsController(
        IDependentRepository dependentRepository,
        IMapper mapper)
    {
        this.dependentRepository = dependentRepository;
        this.mapper = mapper;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await dependentRepository.GetById(id);

        if (dependent == null)
        {
            return NotFound();
        }

        var dependentDto = mapper.Map<GetDependentDto>(dependent);
        return new ApiResponse<GetDependentDto>
        {
            Data = dependentDto,
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await dependentRepository.GetAll();
        var dependentsDto = mapper.Map<List<GetDependentDto>>(dependents);

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependentsDto,
            Success = true
        };

        return result;
    }
}

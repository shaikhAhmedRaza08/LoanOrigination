using LoanOrigination.Application.Dtos;
using LoanOrigination.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanOrigination.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanApplicationsController : ControllerBase
{
    private readonly ILoanApplicationService _service;

    public LoanApplicationsController(ILoanApplicationService service) => _service = service;

    /// <summary>Submit a new loan application. The credit engine runs immediately and returns the decision.</summary>
    [HttpPost]
    public async Task<ActionResult<LoanApplicationResponse>> Submit([FromBody] CreateLoanApplicationRequest request)
    {
        var result = await _service.SubmitAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Fetch a single application by id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LoanApplicationResponse>> GetById(Guid id)
    {
        var result = await _service.GetAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>List all applications (newest first).</summary>
    [HttpGet]
    public async Task<ActionResult<List<LoanApplicationResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Maker-checker: a reviewer approves or rejects a REFERRED application.</summary>
    [HttpPost("{id:guid}/review")]
    public async Task<ActionResult<LoanApplicationResponse>> Review(Guid id, [FromBody] ReviewRequest request)
    {
        var (ok, error, result) = await _service.ReviewAsync(id, request);
        return ok ? Ok(result) : BadRequest(new { error });
    }
}

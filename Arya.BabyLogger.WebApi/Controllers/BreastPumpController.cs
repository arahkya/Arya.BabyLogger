using Arya.BabyLogger.Shared.BreastPump;
using Arya.BabyLogger.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Arya.BabyLogger.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BreastPumpController : ControllerBase
{
    private readonly IBreastPumpService _breastPumpService;

    public BreastPumpController(IBreastPumpService breastPumpService)
    {
        _breastPumpService = breastPumpService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] BreastPumpCreateRequest request)
    {
        if (request.AmountML <= 0)
        {
            return BadRequest("amountML must be greater than 0.");
        }

        var id = await _breastPumpService.CreateAsync(request);
        return CreatedAtRoute("GetBreastPumpById", new { id }, null);
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
        {
            return BadRequest("endDate must be greater than or equal to startDate.");
        }

        var startDateUtc = startDate?.UtcDateTime ?? DateTime.MinValue;
        var endDateUtc = endDate?.UtcDateTime ?? DateTime.MaxValue;

        var response = await _breastPumpService.ListAsync(startDateUtc, endDateUtc);
        return Ok(response);
    }

    [HttpGet("{id:guid}", Name = "GetBreastPumpById")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var response = await _breastPumpService.GetAsync(id);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] BreastPumpUpdateRequest request)
    {
        if (request.AmountML <= 0)
        {
            return BadRequest("amountML must be greater than 0.");
        }

        await _breastPumpService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _breastPumpService.DeleteAsync(id);
        return NoContent();
    }
}

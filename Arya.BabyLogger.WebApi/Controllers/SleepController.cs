using Arya.BabyLogger.Shared.Sleep;
using Arya.BabyLogger.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Arya.BabyLogger.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SleepController : ControllerBase
{
    private readonly ISleepService _sleepService;

    public SleepController(ISleepService sleepService)
    {
        _sleepService = sleepService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] SleepCreateRequest request)
    {
        if (request.EndTime < request.StartTime)
        {
            return BadRequest("endTime must be greater than or equal to startTime.");
        }

        var id = await _sleepService.CreateAsync(request);
        return CreatedAtRoute("GetSleepById", new { id }, null);
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

        var response = await _sleepService.ListAsync(startDateUtc, endDateUtc);
        return Ok(response);
    }

    [HttpGet("{id:guid}", Name = "GetSleepById")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var response = await _sleepService.GetAsync(id);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] SleepUpdateRequest request)
    {
        if (request.EndTime < request.StartTime)
        {
            return BadRequest("endTime must be greater than or equal to startTime.");
        }

        await _sleepService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _sleepService.DeleteAsync(id);
        return NoContent();
    }
}

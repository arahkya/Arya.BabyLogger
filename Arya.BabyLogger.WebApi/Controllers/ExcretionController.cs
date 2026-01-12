using Arya.BabyLogger.Shared.Excretion;
using Arya.BabyLogger.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Arya.BabyLogger.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExcretionController : ControllerBase
{
    private readonly IExcretionService _excretionService;

    public ExcretionController(IExcretionService excretionService)
    {
        _excretionService = excretionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ExcretionCreateRequest request)
    {
        var id = await _excretionService.CreateAsync(request);
        return CreatedAtRoute("GetExcretionById", new { id }, null);
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var startDate = HttpContext.Request.Headers.ContainsKey("Start-Date")
            ? DateTime.Parse(HttpContext.Request.Headers["Start-Date"]!)
            : DateTime.MinValue;
        var endDate = HttpContext.Request.Headers.ContainsKey("End-Date")
            ? DateTime.Parse(HttpContext.Request.Headers["End-Date"]!).Date.AddDays(1).AddTicks(-1)
            : DateTime.MaxValue;

        var response = await _excretionService.ListAsync(startDate, endDate);
        return Ok(response);
    }

    [HttpGet("{id:guid}", Name = "GetExcretionById")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var response = await _excretionService.GetAsync(id);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] ExcretionUpdateRequest request)
    {
        await _excretionService.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _excretionService.DeleteAsync(id);
        return Ok();
    }
}

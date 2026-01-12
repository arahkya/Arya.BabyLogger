namespace Arya.BabyLogger.WebApi.Controllers;

using Arya.BabyLogger.WebApi.Services;
using Arya.BabyLogger.Shared.Feed;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeedEntry([FromBody] CreateFeedEntryRequest request)
    {
        var entryId = await _feedService.CreateFeedEntryAsync(request);

        return CreatedAtAction(nameof(CreateFeedEntry), new { id = entryId }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFeedEntries()
    {
        var startDate = HttpContext.Request.Headers.ContainsKey("Start-Date")
            ? DateTime.Parse(HttpContext.Request.Headers["Start-Date"]!)
            : DateTime.MinValue;
        var endDate = HttpContext.Request.Headers.ContainsKey("End-Date")
            ? DateTime.Parse(HttpContext.Request.Headers["End-Date"]!).Date.AddDays(1).AddTicks(-1)
            : DateTime.MaxValue;

        var entries = await _feedService.GetAllFeedEntriesAsync(startDate, endDate);

        return Ok(entries);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFeedEntryById([FromRoute] Guid id)
    {
        var entry = await _feedService.GetFeedEntryByIdAsync(id);

        if (entry is null)
        {
            return NotFound();
        }

        return Ok(entry);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFeedEntry([FromRoute] Guid id, [FromBody] UpdateFeedEntryRequest request)
    {
        await _feedService.UpdateFeedEntryAsync(id, request);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFeedEntry([FromRoute] Guid id)
    {
        await _feedService.DeleteFeedEntryAsync(id);

        return Ok();
    }
}

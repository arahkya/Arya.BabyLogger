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
        var entries = await _feedService.GetAllFeedEntriesAsync();

        return Ok(entries);
    }
}

using CarNotesAPI.Data.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("action-records/{carId}")]
    public async Task<int> GetNumberOfActionRecords(Guid carId)
    {
        return await _statsService.NumberOfActionRecords(carId);
    }
}


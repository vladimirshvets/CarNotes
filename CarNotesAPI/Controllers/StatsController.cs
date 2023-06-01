using CarNotesAPI.Data.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("total-cars")]
    public async Task<int> TotalNumberOfCars()
    {
        return await _statsService.TotalNumberOfCars();
    }

    [HttpGet("total-users")]
    public async Task<int> TotalNumberOfUsers()
    {
        return await _statsService.TotalNumberOfUsers();
    }

    [Authorize]
    [HttpGet("action-records/{carId}")]
    public async Task<int> GetNumberOfActionRecords(Guid carId)
    {
        return await _statsService.NumberOfActionRecords(carId);
    }
}


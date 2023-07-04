using CarNotes.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Route("api/global-stats")]
public class GlobalStatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public GlobalStatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("total-cars")]
    public async Task<int> TotalNumberOfCars()
        => await _statsService.TotalNumberOfCarsAsync();

    [HttpGet("total-users")]
    public async Task<int> TotalNumberOfUsers()
        => await _statsService.TotalNumberOfUsersAsync();
}

using CarNotes.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/personal-stats")]
public class PersonalStatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public PersonalStatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("action-records/{carId}")]
    public async Task<int> GetNumberOfActionRecords(Guid carId)
        => await _statsService.NumberOfActionRecords(carId);

    [HttpGet("average-consumption/{carId}")]
    public async Task<double> GetAverageFuelConsumption(Guid carId)
        => await _statsService.AverageFuelConsumption(carId);

    [HttpGet("odometer-delta/{carId}")]
    public async Task<int> GetOdometerDelta(Guid carId)
        => await _statsService.OdometerDelta(carId);

    [HttpGet("money-spendings/{carId:guid}")]
    public async Task<IActionResult> GetMoneySpendingsStats(Guid carId)
    {
        var moneyTotalTask = _statsService.MoneySpentInTotal(carId);
        var moneyPerKmTask = _statsService.MoneySpentPerKm(carId);
        var moneyPerMonthTask = _statsService.MoneySpentPerMonth(carId);

        // Run tasks in parallel.
        await Task.WhenAll(moneyTotalTask, moneyPerKmTask, moneyPerMonthTask);

        return Ok(
            new
            {
                MoneyTotal = await moneyTotalTask,
                MoneyPerKm = await moneyPerKmTask,
                MoneyPerMonth = await moneyPerMonthTask
            }
        );
    }
}

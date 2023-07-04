using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models.Notes;
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

    [HttpGet("money-spendings/details/{carId:guid}")]
    public async Task<IActionResult> GetMoneySpendingsDetailedStats(Guid carId)
    {
        var legalProcedureSpendingsTask =
            _statsService.MoneySpentByNoteType(carId, nameof(LegalProcedure));
        var refuelingSpendingsTask =
            _statsService.MoneySpentByNoteType(carId, nameof(Refueling));
        var serviceSpendingsTask =
            _statsService.MoneySpentByNoteType(carId, nameof(Service));
        var sparePartSpendingsTask =
            _statsService.MoneySpentByNoteType(carId, nameof(SparePart));
        var washingSpendingsTask =
            _statsService.MoneySpentByNoteType(carId, nameof(Washing));

        await Task.WhenAll(
            legalProcedureSpendingsTask,
            refuelingSpendingsTask,
            serviceSpendingsTask,
            sparePartSpendingsTask,
            washingSpendingsTask
        );

        return Ok(
            new
            {
                LegalProceduresTotal = await legalProcedureSpendingsTask,
                RefuelingsTotal = await refuelingSpendingsTask,
                ServicesTotal = await serviceSpendingsTask,
                SparePartsTotal = await sparePartSpendingsTask,
                WashingsTotal = await washingSpendingsTask
            }
        );
    }
}

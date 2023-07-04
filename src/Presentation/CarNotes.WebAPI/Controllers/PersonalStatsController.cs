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
        => await _statsService.NumberOfActionRecordsAsync(carId);

    [HttpGet("distance/{carId:guid}")]
    public async Task<IActionResult> GetDistanceStats(Guid carId)
    {
        var totalDistanceTask = _statsService.TotalDistanceAsync(carId);
        var distancePerMonthTask = _statsService.DistancePerMonthAsync(carId);
        var avgConsumptionTask = _statsService.AverageFuelConsumptionAsync(carId);

        await Task.WhenAll(
            totalDistanceTask,
            distancePerMonthTask,
            avgConsumptionTask
        );

        return Ok(
            new
            {
                TotalDistance = await totalDistanceTask,
                DistancePerMonth = await distancePerMonthTask,
                AverageFuelConsumption = await avgConsumptionTask
            }
        );
    }

    [HttpGet("money-spendings/{carId:guid}")]
    public async Task<IActionResult> GetMoneySpendingsStats(Guid carId)
    {
        var moneyTotalTask = _statsService.MoneySpentInTotalAsync(carId);
        var moneyPerKmTask = _statsService.MoneySpentPerKmAsync(carId);
        var moneyPerMonthTask = _statsService.MoneySpentPerMonthAsync(carId);

        await Task.WhenAll(
            moneyTotalTask,
            moneyPerKmTask,
            moneyPerMonthTask
        );

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
            _statsService.MoneySpentByNoteTypeAsync(carId, nameof(LegalProcedure));
        var refuelingSpendingsTask =
            _statsService.MoneySpentByNoteTypeAsync(carId, nameof(Refueling));
        var serviceSpendingsTask =
            _statsService.MoneySpentByNoteTypeAsync(carId, nameof(Service));
        var sparePartSpendingsTask =
            _statsService.MoneySpentByNoteTypeAsync(carId, nameof(SparePart));
        var washingSpendingsTask =
            _statsService.MoneySpentByNoteTypeAsync(carId, nameof(Washing));

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

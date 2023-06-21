using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class RefuelingsController : ControllerBase
{
    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<Refueling> _refuelingRepository;

    public RefuelingsController(
        IMileageRepository mileageRepository,
        INoteRepository<Refueling> refuelingRepository)
    {
        _mileageRepository = mileageRepository;
        _refuelingRepository = refuelingRepository;
    }

    [HttpGet]
    [Route("getByCar/{carId}")]
    public async Task<IEnumerable<Refueling>> GetByCar(Guid carId)
    {
        return await _refuelingRepository.GetListAsync(carId);
    }

    [HttpPost]
    public async Task<Refueling> Post([FromBody] RefuelingViewModel viewModel)
    {
        Mileage mileage = viewModel.Mileage;
        if (mileage.Id == Guid.Empty)
        {
            mileage = await _mileageRepository.AddAsync(
                viewModel.CarId, viewModel.Mileage);
        }

        Refueling refueling = new()
        {
            Volume = viewModel.Volume,
            Price = viewModel.Price,
            Distributor = viewModel.Distributor,
            Address = viewModel.Address,
            Comment = viewModel.Comment
        };
        refueling = await _refuelingRepository.AddAsync(
            viewModel.CarId, mileage.Id, refueling);

        return refueling;
    }

    [HttpPut("{id}")]
    public async Task<Refueling> Put(
        Guid id, [FromBody] RefuelingViewModel viewModel)
    {
        Refueling refueling = new()
        {
            Volume = viewModel.Volume,
            Price = viewModel.Price,
            Distributor = viewModel.Distributor,
            Address = viewModel.Address,
            Comment = viewModel.Comment
        };
        refueling = await _refuelingRepository.UpdateAsync(
            viewModel.CarId, viewModel.Mileage.Id, id, refueling);

        return refueling;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id, [FromBody] RefuelingViewModel viewModel)
    {
        await _refuelingRepository.DeleteAsync(
            viewModel.CarId, viewModel.Mileage.Id, id);

        bool isMileageDeleted = false;
        int relatedRecords =
            await _mileageRepository.GetRelatedRecordsCountAsync(
                viewModel.CarId, viewModel.Mileage.Id);
        if (relatedRecords == 0)
        {
            await _mileageRepository.DeleteAsync(
                viewModel.CarId, viewModel.Mileage.Id);
            isMileageDeleted = true;
        }

        return Ok(new
        {
            Id = id,
            IsMileageDeleted = isMileageDeleted,
            MileageId = viewModel.Mileage.Id
        });
    }
}

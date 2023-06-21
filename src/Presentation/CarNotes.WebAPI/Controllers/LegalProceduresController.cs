using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class LegalProceduresController : ControllerBase
{
    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<LegalProcedure> _legalProcedureRepository;

    public LegalProceduresController(
        IMileageRepository mileageRepository,
        INoteRepository<LegalProcedure> legalProcedureRepository)
    {
        _mileageRepository = mileageRepository;
        _legalProcedureRepository = legalProcedureRepository;
    }

    [HttpGet]
    [Route("getByCar/{carId}")]
    public async Task<IEnumerable<LegalProcedure>> GetByCar(Guid carId)
    {
        return await _legalProcedureRepository.GetListAsync(carId);
    }

    [HttpPost]
    public async Task<LegalProcedure> Post(
        [FromBody] LegalProcedureViewModel viewModel)
    {
        Mileage mileage = viewModel.Mileage;
        if (mileage.Id == Guid.Empty)
        {
            mileage = await _mileageRepository.AddAsync(
                viewModel.CarId, viewModel.Mileage);
        }

        LegalProcedure legalProcedure = new()
        {
            Group = viewModel.Group,
            Title = viewModel.Title,
            TotalAmount = viewModel.TotalAmount,
            ExpirationDate = viewModel.ExpirationDate,
            Comment = viewModel.Comment
        };
        legalProcedure = await _legalProcedureRepository.AddAsync(
            viewModel.CarId, mileage.Id, legalProcedure);

        return legalProcedure;
    }

    [HttpPut("{id}")]
    public async Task<LegalProcedure> Put(
        Guid id, [FromBody] LegalProcedureViewModel viewModel)
    {
        LegalProcedure legalProcedure = new()
        {
            Group = viewModel.Group,
            Title = viewModel.Title,
            TotalAmount = viewModel.TotalAmount,
            ExpirationDate = viewModel.ExpirationDate,
            Comment = viewModel.Comment
        };
        legalProcedure = await _legalProcedureRepository.UpdateAsync(
            viewModel.CarId, viewModel.Mileage.Id, id, legalProcedure);

        return legalProcedure;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id, [FromBody] LegalProcedureViewModel viewModel)
    {
        await _legalProcedureRepository.DeleteAsync(
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

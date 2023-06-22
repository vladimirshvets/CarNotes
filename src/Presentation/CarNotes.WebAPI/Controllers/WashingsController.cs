using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class WashingsController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<Washing> _washingRepository;

    public WashingsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<Washing> washingRepository)
    {
        _mapper = mapper;
        _mileageRepository = mileageRepository;
        _washingRepository = washingRepository;
    }

    [HttpGet]
    [Route("getByCar/{carId}")]
    public async Task<IEnumerable<Washing>> GetByCar(Guid carId)
    {
        return await _washingRepository.GetListAsync(carId);
    }

    [HttpPost]
    public async Task<Washing> Post([FromBody] WashingViewModel viewModel)
    {
        Mileage mileage = viewModel.Mileage;
        if (mileage.Id == Guid.Empty)
        {
            mileage = await _mileageRepository.AddAsync(
                viewModel.CarId, viewModel.Mileage);
        }

        Washing washing = _mapper.Map<Washing>(viewModel);
        washing = await _washingRepository.AddAsync(
            viewModel.CarId, mileage.Id, washing);

        return washing;
    }

    [HttpPut("{id}")]
    public async Task<Washing> Put(
        Guid id, [FromBody] WashingViewModel viewModel)
    {
        Washing washing = _mapper.Map<Washing>(viewModel);
        washing = await _washingRepository.UpdateAsync(
            viewModel.CarId, viewModel.Mileage.Id, id, washing);

        return washing;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id, [FromBody] WashingViewModel viewModel)
    {
        await _washingRepository.DeleteAsync(
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

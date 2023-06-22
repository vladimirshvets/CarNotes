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
public class SparePartsController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<SparePart> _sparePartRepository;

    public SparePartsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<SparePart> sparePartRepository)
    {
        _mapper = mapper;
        _mileageRepository = mileageRepository;
        _sparePartRepository = sparePartRepository;
    }

    [HttpGet]
    [Route("getByCar/{carId}")]
    public async Task<IEnumerable<SparePart>> GetByCar(Guid carId)
    {
        return await _sparePartRepository.GetListAsync(carId);
    }

    [HttpPost]
    public async Task<SparePart> Post([FromBody] SparePartViewModel viewModel)
    {
        Mileage mileage = viewModel.Mileage;
        if (mileage.Id == Guid.Empty)
        {
            mileage = await _mileageRepository.AddAsync(
                viewModel.CarId, viewModel.Mileage);
        }

        SparePart sparePart = _mapper.Map<SparePart>(viewModel);
        sparePart = await _sparePartRepository.AddAsync(
            viewModel.CarId, mileage.Id, sparePart);

        return sparePart;
    }

    [HttpPut("{id}")]
    public async Task<SparePart> Put(
        Guid id, [FromBody] SparePartViewModel viewModel)
    {
        SparePart sparePart = _mapper.Map<SparePart>(viewModel);
        sparePart = await _sparePartRepository.UpdateAsync(
            viewModel.CarId, viewModel.Mileage.Id, id, sparePart);

        return sparePart;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id, [FromBody] SparePartViewModel viewModel)
    {
        await _sparePartRepository.DeleteAsync(
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

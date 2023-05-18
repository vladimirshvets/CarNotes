using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<Service> _serviceRepository;

    public ServicesController(
        IMileageRepository mileageRepository,
        INoteRepository<Service> serviceRepository)
    {
        _mileageRepository = mileageRepository;
        _serviceRepository = serviceRepository;
    }

    [HttpGet]
    [Route("getByCar/{carId}")]
    public async Task<IEnumerable<Service>> GetByCar(Guid carId)
    {
        return await _serviceRepository.GetListAsync(carId);
    }

    [HttpPost]
    public async Task<Service> Post([FromBody] ServiceViewModel viewModel)
    {
        Mileage mileage = viewModel.Mileage;
        if (mileage.Id == Guid.Empty)
        {
            mileage = await _mileageRepository.AddAsync(
                viewModel.CarId, viewModel.Mileage);
        }

        Service service = new()
        {
            Title = viewModel.Title,
            StationName = viewModel.StationName,
            Address = viewModel.Address,
            WebsiteUrl = viewModel.WebsiteUrl,
            CostOfWork = viewModel.CostOfWork,
            CostOfSpareParts = viewModel.CostOfSpareParts,
            Comment = viewModel.Comment
        };
        service = await _serviceRepository.AddAsync(
            viewModel.CarId, mileage.Id, service);

        return service;
    }

    [HttpPut("{id}")]
    public async Task<Service> Put(
        Guid id, [FromBody] ServiceViewModel viewModel)
    {
        Service service = new()
        {
            Title = viewModel.Title,
            StationName = viewModel.StationName,
            Address = viewModel.Address,
            WebsiteUrl = viewModel.WebsiteUrl,
            CostOfWork = viewModel.CostOfWork,
            CostOfSpareParts = viewModel.CostOfSpareParts,
            Comment = viewModel.Comment
        };
        service = await _serviceRepository.UpdateAsync(
            viewModel.CarId, viewModel.Mileage.Id, id, service);

        return service;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id, [FromBody] ServiceViewModel viewModel)
    {
        await _serviceRepository.DeleteAsync(
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

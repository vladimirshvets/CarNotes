﻿using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<Service> _serviceRepository;

    public ServicesController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<Service> serviceRepository)
    {
        _mapper = mapper;
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

        Service service = _mapper.Map<Service>(viewModel);
        service = await _serviceRepository.AddAsync(
            viewModel.CarId, mileage.Id, service);

        return service;
    }

    [HttpPut("{id}")]
    public async Task<Service> Put(
        Guid id, [FromBody] ServiceViewModel viewModel)
    {
        Service service = _mapper.Map<Service>(viewModel);
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

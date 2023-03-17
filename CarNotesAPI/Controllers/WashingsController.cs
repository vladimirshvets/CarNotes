﻿using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Route("api/[controller]")]
    public class WashingsController : ControllerBase
    {
        private readonly MileageRepository _mileageRepository;

        private readonly WashingRepository _washingRepository;

        public WashingsController(
            MileageRepository mileageRepository,
            WashingRepository washingRepository)
        {
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
        public async Task<Washing> Post([FromBody]WashingViewModel viewModel)
        {
            // ToDo:
            // Check if the mileage already exists.
            // Try to save mileage and refueling together.
            Mileage mileage = new()
            {
                OdometerValue = viewModel.OdometerValue,
                Date = viewModel.Date
            };
            mileage =
                await _mileageRepository.AddAsync(viewModel.CarId, mileage);

            Washing washing = new()
            {
                Title = viewModel.Title,
                Address = viewModel.Address,
                IsContact = viewModel.IsContact,
                IsDegreaserUsed = viewModel.IsDegreaserUsed,
                IsPolishUsed = viewModel.IsPolishUsed,
                IsAntiRainUsed = viewModel.IsAntiRainUsed,
                TotalAmount = viewModel.TotalAmount
            };
            washing = await _washingRepository.AddAsync(
                viewModel.CarId, mileage.Id, washing);

            return washing;
        }
    }
}

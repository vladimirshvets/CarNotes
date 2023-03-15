using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Route("api/[controller]")]
    public class WashingsController : Controller
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
        public async Task<Washing> Post([FromBody]WashingViewModel washingVM)
        {
            // ToDo:
            // Check if the mileage already exists.
            // Try to save mileage and refueling together.
            Mileage mileage = new()
            {
                OdometerValue = washingVM.OdometerValue,
                Date = washingVM.Date

            };
            mileage = await _mileageRepository.AddAsync(washingVM.CarId, mileage);

            Washing washing = new()
            {
                Title = washingVM.Title,
                Address = washingVM.Address,
                IsContact = washingVM.IsContact,
                IsDegreaserUsed = washingVM.IsDegreaserUsed,
                IsPolishUsed = washingVM.IsPolishUsed,
                IsAntiRainUsed = washingVM.IsAntiRainUsed,
                TotalAmount = washingVM.TotalAmount
            };
            washing = await _washingRepository.AddAsync(
                washingVM.CarId, mileage.Id, washing);

            return washing;
        }
    }
}


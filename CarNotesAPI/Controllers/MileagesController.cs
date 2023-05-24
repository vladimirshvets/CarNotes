﻿using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MileagesController : ControllerBase
    {
        private readonly IMileageRepository _mileageRepository;

        public MileagesController(IMileageRepository mileageRepository)
        {
            _mileageRepository = mileageRepository;
        }

        [HttpGet]
        [Route("getByCar/{carId}")]
        public async Task<IEnumerable<Mileage>> GetByCar(Guid carId)
        {
            return await _mileageRepository.GetListAsync(carId);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(
            [FromBody]NoteViewModel viewModel)
        {
            Mileage mileage = viewModel.Mileage;
            if (mileage.Id != Guid.Empty)
            {
                return Conflict();
            }
            mileage =
                await _mileageRepository.AddAsync(viewModel.CarId, mileage);
            return Ok(mileage);
        }

        [HttpPut]
        public async Task<Mileage> Update(
            Guid carId, DateOnly mileageDate, int mileageValue,
            [FromBody]Mileage mileage)
        {
            return await _mileageRepository.UpdateAsync(
                carId, mileageDate, mileageValue, mileage);
        }
    }
}

using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers
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
        [Route("getByCar/{carId:guid}")]
        public async Task<IEnumerable<Mileage>> GetByCar(Guid carId)
        {
            return await _mileageRepository.GetListAsync(carId);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post(
            [FromBody] NoteDto dto)
        {
            Mileage mileage = dto.Mileage;
            if (mileage.Id != Guid.Empty)
            {
                return Conflict();
            }
            mileage =
                await _mileageRepository.AddAsync(dto.CarId, mileage);
            return Ok(mileage);
        }

        [HttpPut]
        public async Task<Mileage> Update(
            Guid carId, DateOnly mileageDate, int mileageValue,
            [FromBody] Mileage mileage)
        {
            return await _mileageRepository.UpdateAsync(
                carId, mileageDate, mileageValue, mileage);
        }
    }
}

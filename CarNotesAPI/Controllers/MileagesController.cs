using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Route("api/[controller]")]
    public class MileagesController : ControllerBase
    {
        private readonly MileageRepository _mileageRepository;

        public MileagesController(MileageRepository mileageRepository)
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
        public async Task<Mileage> Post([FromBody]MileageViewModel viewModel)
        {
            Mileage mileage = new()
            {
                OdometerValue = viewModel.OdometerValue,
                Date = viewModel.Date
            };
            return await _mileageRepository.AddAsync(viewModel.CarId, mileage);
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

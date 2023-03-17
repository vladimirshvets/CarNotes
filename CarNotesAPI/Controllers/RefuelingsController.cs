using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Route("api/[controller]")]
    public class RefuelingsController : ControllerBase
    {
        private readonly MileageRepository _mileageRepository;

        private readonly RefuelingRepository _refuelingRepository;

        public RefuelingsController(
            MileageRepository mileageRepository,
            RefuelingRepository refuelingRepository)
        {
            _mileageRepository = mileageRepository;
            _refuelingRepository = refuelingRepository;
        }

        [HttpGet]
        [Route("getByCar/{carId}")]
        public async Task<IEnumerable<Refueling>> GetByCar(Guid carId)
        {
            return await _refuelingRepository.GetListAsync(carId);
        }

        [HttpPost]
        public async Task<Refueling> Post(
            [FromBody]RefuelingViewModel viewModel)
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

            Refueling refueling = new()
            {
                Volume = viewModel.Volume,
                Price = viewModel.Price,
                Distributor = viewModel.Distributor,
                Address = viewModel.Address,
                Comment = viewModel.Comment

            };
            refueling = await _refuelingRepository.AddAsync(
                viewModel.CarId, mileage.Id, refueling);

            return refueling;
        }
    }
}

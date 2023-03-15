using CarNotesAPI.Data.Models;
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

        [HttpPost]
        public async Task<Refueling> Post([FromBody] RefuelingViewModel refuelingVM)
        {
            // ToDo:
            // Check if the mileage already exists.
            // Try to save mileage and refueling together.
            Mileage mileage = new()
            {
                OdometerValue = refuelingVM.OdometerValue,
                Date = refuelingVM.Date

            };
            mileage = await _mileageRepository.AddAsync(refuelingVM.CarId, mileage);

            Refueling refueling = new()
            {
                Volume = refuelingVM.Volume,
                Price = refuelingVM.Price,
                Distributor = refuelingVM.Distributor,
                Address = refuelingVM.Address,
                Comment = refuelingVM.Comment

            };
            refueling = await _refuelingRepository.AddAsync(
                refuelingVM.CarId, mileage.Id, refueling);

            return refueling;
        }
    }
}

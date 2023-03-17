using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly MileageRepository _mileageRepository;

        private readonly ServiceRepository _serviceRepository;

        public ServicesController(
            MileageRepository mileageRepository,
            ServiceRepository serviceRepository)
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
        public async Task<Service> Post([FromBody]ServiceViewModel viewModel)
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

            Service service = new()
            {
                Title = viewModel.Title,
                StationName = viewModel.StationName,
                Address = viewModel.Address,
                WebsiteUrl = viewModel.WebsiteUrl,
                CostOfWork = viewModel.CostOfWork,
                CostOfSpareParts = viewModel.CostOfSpareParts
            };
            service = await _serviceRepository.AddAsync(
                viewModel.CarId, mileage.Id, service);

            return service;
        }
    }
}

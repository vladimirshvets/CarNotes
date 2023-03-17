using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers
{
    [Route("api/[controller]")]
    public class SparePartsController : ControllerBase
    {
        private readonly MileageRepository _mileageRepository;

        private readonly SparePartRepository _sparePartRepository;

        public SparePartsController(
            MileageRepository mileageRepository,
            SparePartRepository sparePartRepository)
        {
            _mileageRepository = mileageRepository;
            _sparePartRepository = sparePartRepository;
        }

        [HttpGet]
        [Route("getByCar/{carId}")]
        public async Task<IEnumerable<SparePart>> GetByCar(Guid carId)
        {
            return await _sparePartRepository.GetListAsync(carId);
        }

        [HttpPost]
        public async Task<SparePart> Post([FromBody]SparePartViewModel viewModel)
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

            SparePart sparePart = new()
            {
                Category            = viewModel.Category,
                OrderDate           = viewModel.OrderDate,
                PurchaseDate        = viewModel.PurchaseDate,
                Group               = viewModel.Group,
                Name                = viewModel.Name,
                UoM                 = viewModel.UoM,
                IsOE                = viewModel.IsOE,
                OENumber            = viewModel.OENumber,
                ReplacementNumber   = viewModel.ReplacementNumber,
                Manufacturer        = viewModel.Manufacturer,
                CountryOfOrigin     = viewModel.CountryOfOrigin,
                Price               = viewModel.Price,
                Qty                 = viewModel.Qty,
                ShopWebsiteUrl      = viewModel.ShopWebsiteUrl,
                ShopAddress         = viewModel.ShopAddress,
                ProductionDate      = viewModel.ProductionDate,
                ExpirationDate      = viewModel.ExpirationDate
            };
            sparePart = await _sparePartRepository.AddAsync(
                viewModel.CarId, mileage.Id, sparePart);

            return sparePart;
        }
    }
}

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
    public class LegalProceduresController : ControllerBase
    {
        private readonly MileageRepository _mileageRepository;

        private readonly LegalProcedureRepository _legalProcedureRepository;

        public LegalProceduresController(
            MileageRepository mileageRepository,
            LegalProcedureRepository legalProcedureRepository)
        {
            _mileageRepository = mileageRepository;
            _legalProcedureRepository = legalProcedureRepository;
        }

        [HttpGet]
        [Route("getByCar/{carId}")]
        public async Task<IEnumerable<LegalProcedure>> GetByCar(Guid carId)
        {
            return await _legalProcedureRepository.GetListAsync(carId);
        }

        [HttpPost]
        public async Task<LegalProcedure> Post(
            [FromBody]LegalProcedureViewModel viewModel)
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

            LegalProcedure legalProcedure = new()
            {
                Group = viewModel.Group,
                Title = viewModel.Title,
                TotalAmount = viewModel.TotalAmount,
                ExpirationDate = viewModel.ExpirationDate
            };
            legalProcedure = await _legalProcedureRepository.AddAsync(
                viewModel.CarId, mileage.Id, legalProcedure);

            return legalProcedure;
        }
    }
}

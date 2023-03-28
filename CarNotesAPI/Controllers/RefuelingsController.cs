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
            Mileage mileage = viewModel.Mileage;
            if (mileage.Id == Guid.Empty)
            {
                mileage = await _mileageRepository.AddAsync(
                    viewModel.CarId, viewModel.Mileage);
            }

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

        [HttpPut("{id}")]
        public async Task<Refueling> Put(
            Guid id, [FromBody]RefuelingViewModel viewModel)
        {
            Refueling refueling = new()
            {
                Volume = viewModel.Volume,
                Price = viewModel.Price,
                Distributor = viewModel.Distributor,
                Address = viewModel.Address,
                Comment = viewModel.Comment
            };

            refueling = await _refuelingRepository.UpdateAsync(
                viewModel.CarId, viewModel.Mileage.Id, id, refueling);

            return refueling;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id, [FromBody]RemovalViewModel viewModel)
        {
            await _refuelingRepository.DeleteAsync(
                viewModel.CarId, viewModel.MileageId, id);

            bool isMileageDeleted = false;
            int relatedRecords =
                await _mileageRepository.GetRelatedRecordsCountAsync(
                    viewModel.CarId, viewModel.MileageId);
            if (relatedRecords == 0)
            {
                await _mileageRepository.DeleteAsync(
                    viewModel.CarId, viewModel.MileageId);
                isMileageDeleted = true;
            }

            return Ok(new
            {
                RefuelingId = id,
                IsMileageDeleted = isMileageDeleted,
                MileageId = viewModel.MileageId
            });
        }
    }
}

using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI.Controllers.Notes;

public class RefuelingsController : NotesController<Refueling, RefuelingDto>
{
    public RefuelingsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<Refueling> refuelingRepository)
        : base(mapper, mileageRepository, refuelingRepository)
    {
    }
}

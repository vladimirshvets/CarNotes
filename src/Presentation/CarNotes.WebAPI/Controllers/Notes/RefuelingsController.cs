using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;

namespace CarNotes.WebAPI.Controllers.Notes;

public class RefuelingsController : NotesController<Refueling, RefuelingViewModel>
{
    public RefuelingsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<Refueling> refuelingRepository)
        : base(mapper, mileageRepository, refuelingRepository)
    {
    }
}

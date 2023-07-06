using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI.Controllers.Notes;

public class WashingsController : NotesController<Washing, WashingDto>
{
    public WashingsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<Washing> washingRepository)
        : base(mapper, mileageRepository, washingRepository)
    {
    }
}

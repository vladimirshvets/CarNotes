using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI.Controllers.Notes;

public class SparePartsController : NotesController<SparePart, SparePartDto>
{
    public SparePartsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<SparePart> sparePartRepository)
        : base(mapper, mileageRepository, sparePartRepository)
    {
    }
}

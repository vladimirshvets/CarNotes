using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;

namespace CarNotes.WebAPI.Controllers.Notes;

public class SparePartsController : NotesController<SparePart, SparePartViewModel>
{
    public SparePartsController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<SparePart> sparePartRepository)
        : base(mapper, mileageRepository, sparePartRepository)
    {
    }
}

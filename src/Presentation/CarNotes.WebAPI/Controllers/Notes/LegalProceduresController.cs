using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;

namespace CarNotes.WebAPI.Controllers.Notes;

public class LegalProceduresController : NotesController<LegalProcedure, LegalProcedureViewModel>
{
    public LegalProceduresController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<LegalProcedure> legalProcedureRepository)
        : base(mapper, mileageRepository, legalProcedureRepository)
    {
    }
}

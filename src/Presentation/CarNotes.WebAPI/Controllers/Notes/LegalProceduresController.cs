using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI.Controllers.Notes;

public class LegalProceduresController : NotesController<LegalProcedure, LegalProcedureDto>
{
    public LegalProceduresController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<LegalProcedure> legalProcedureRepository)
        : base(mapper, mileageRepository, legalProcedureRepository)
    {
    }
}

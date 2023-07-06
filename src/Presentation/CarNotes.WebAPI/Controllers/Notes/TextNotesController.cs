using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI.Controllers.Notes;

public class TextNotesController : NotesController<TextNote, TextNoteDto>
{
    public TextNotesController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<TextNote> textNoteRepository)
        : base(mapper, mileageRepository, textNoteRepository)
    {
    }
}

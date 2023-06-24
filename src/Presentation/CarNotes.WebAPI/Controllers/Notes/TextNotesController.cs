using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;

namespace CarNotes.WebAPI.Controllers.Notes;

public class TextNotesController : NotesController<TextNote, TextNoteViewModel>
{
    public TextNotesController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<TextNote> textNoteRepository)
        : base(mapper, mileageRepository, textNoteRepository)
    {
    }
}

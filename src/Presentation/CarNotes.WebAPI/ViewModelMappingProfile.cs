using AutoMapper;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.ViewModels;

namespace CarNotes.WebAPI;

public class ViewModelMappingProfile : Profile
{
    public ViewModelMappingProfile()
    {
        CreateMap<LegalProcedureViewModel, LegalProcedure>();
        CreateMap<RefuelingViewModel, Refueling>();
        CreateMap<ServiceViewModel, Service>();
        CreateMap<SparePartViewModel, SparePart>();
        CreateMap<TextNoteViewModel, TextNote>();
        CreateMap<WashingViewModel, Washing>();
    }
}

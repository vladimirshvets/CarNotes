using AutoMapper;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<LegalProcedureDto, LegalProcedure>();
        CreateMap<RefuelingDto, Refueling>();
        CreateMap<ServiceDto, Service>();
        CreateMap<SparePartDto, SparePart>();
        CreateMap<TextNoteDto, TextNote>();
        CreateMap<WashingDto, Washing>();
    }
}

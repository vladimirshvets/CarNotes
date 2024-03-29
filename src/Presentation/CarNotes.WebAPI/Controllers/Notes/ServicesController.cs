﻿using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models.Notes;
using CarNotes.WebAPI.Models;

namespace CarNotes.WebAPI.Controllers.Notes;

public class ServicesController : NotesController<Service, ServiceDto>
{
    public ServicesController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<Service> serviceRepository)
        : base(mapper, mileageRepository, serviceRepository)
    {
    }
}

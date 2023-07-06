using AutoMapper;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using CarNotes.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public abstract class NotesController<T, V> : ControllerBase
    where T : Note
    where V : NoteDto
{
    private readonly IMapper _mapper;

    private readonly IMileageRepository _mileageRepository;

    private readonly INoteRepository<T> _noteRepository;

    public NotesController(
        IMapper mapper,
        IMileageRepository mileageRepository,
        INoteRepository<T> noteRepository)
    {
        _mapper = mapper;
        _mileageRepository = mileageRepository;
        _noteRepository = noteRepository;
    }

    [HttpGet]
    [Route("getByCar/{carId:guid}")]
    public async Task<IEnumerable<T>> GetByCar(Guid carId)
    {
        return await _noteRepository.GetListAsync(carId);
    }

    [HttpPost]
    public async Task<T> Post([FromBody] V dto)
    {
        Mileage mileage = dto.Mileage;
        if (mileage.Id == Guid.Empty)
        {
            mileage = await _mileageRepository.AddAsync(
                dto.CarId, dto.Mileage);
        }

        T note = _mapper.Map<T>(dto);
        note = await _noteRepository.AddAsync(dto.CarId, mileage.Id, note);

        return note;
    }

    [HttpPut("{id:guid}")]
    public async Task<T> Put(Guid id, [FromBody] V dto)
    {
        T note = _mapper.Map<T>(dto);
        note = await _noteRepository.UpdateAsync(
            dto.CarId, dto.Mileage.Id, id, note);

        return note;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromBody] V dto)
    {
        await _noteRepository.DeleteAsync(
            dto.CarId, dto.Mileage.Id, id);

        bool isMileageDeleted = false;
        int relatedRecords =
            await _mileageRepository.GetRelatedRecordsCountAsync(
                dto.CarId, dto.Mileage.Id);
        if (relatedRecords == 0)
        {
            await _mileageRepository.DeleteAsync(
                dto.CarId, dto.Mileage.Id);
            isMileageDeleted = true;
        }

        return Ok(new
        {
            Id = id,
            IsMileageDeleted = isMileageDeleted,
            MileageId = dto.Mileage.Id
        });
    }
}


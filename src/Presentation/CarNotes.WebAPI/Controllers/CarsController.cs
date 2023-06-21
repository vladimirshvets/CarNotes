using System.Security.Claims;
using CarNotes.Domain.Interfaces.Repositories;
using CarNotes.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    public CarsController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<Car>> GetList()
    {
        Guid userId = GetUserId();
        return await _carRepository.GetListAsync(userId);
    }

    [HttpGet]
    [Route("{carId}")]
    public async Task<ActionResult<Car>> Get(Guid carId)
    {
        Guid userId = GetUserId();
        Car? car = await _carRepository.GetAsync(userId, carId);
        if (car == null)
        {
            return BadRequest();
        }
        return car;
    }

    [HttpPost]
    public async Task<ActionResult<Car>> Post([FromBody] Car car)
    {
        Guid userId = GetUserId();
        return await _carRepository.AddAsync(userId, car);
    }

    [HttpPut("{carId}")]
    public async Task<Car> Put(Guid carId, [FromBody] Car car)
    {
        return await _carRepository.UpdateAsync(carId, car);
    }

    [HttpDelete("{carId}")]
    public async Task<ActionResult<bool>> Delete(Guid carId)
    {
        Guid userId = GetUserId();
        return await _carRepository.DeleteAsync(userId, carId);
    }

    private Guid GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var nameIdentifierClaim = identity?.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.NameIdentifier);

        if (nameIdentifierClaim != null)
        {
            if (Guid.TryParse(nameIdentifierClaim.Value, out Guid userId))
            {
                return userId;
            }
        }

        throw new NotImplementedException();
    }
}

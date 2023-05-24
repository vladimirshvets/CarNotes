using System.Security.Claims;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    // ToDo:
    // get from storage when a user functionality is ready.
    private readonly Guid UserId =
        new("48898106-b1b2-4bc1-8f63-3ee777fdeafd");

    public CarsController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<Car>?> GetList()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var nameIdentifierClaim = identity.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.NameIdentifier);

        if (nameIdentifierClaim != null)
        {
            if (Guid.TryParse(nameIdentifierClaim.Value, out Guid userId))
            {
                return await _carRepository.GetListAsync(userId);
            }
        }

        return null;
    }

    [HttpGet]
    [Route("{carId}")]
    public async Task<ActionResult<Car>> Get(Guid carId)
    {
        Car? car = await _carRepository.GetAsync(UserId, carId);
        if (car == null)
        {
            return BadRequest();
        }
        return car;
    }

    [HttpPost]
    public async Task<Car> Post([FromBody]Car car)
    {
        return await _carRepository.AddAsync(UserId, car);
    }

    [HttpPut("{id}")]
    public async Task<Car> Put(Guid id, [FromBody]Car car)
    {
        return await _carRepository.UpdateAsync(id, car);
    }
}

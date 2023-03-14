using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private CarRepository _carRepository;

    public CarsController(CarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet]
    [Route("getByUser")]
    public async Task<IEnumerable<Car>> GetByUser()
    {
        Guid userId = new Guid("48898106-b1b2-4bc1-8f63-3ee777fdeafd");
        return await _carRepository.GetByUser(userId);
    }

    [HttpPost]
    public async Task<bool> Post([FromBody]Car car)
    {
        Guid userId = new Guid("48898106-b1b2-4bc1-8f63-3ee777fdeafd");
        return await _carRepository.AddAsync(userId, car);
    }

    [HttpPut("{id}")]
    public async Task<Car> Put(Guid id, [FromBody]Car car)
    {
        return await _carRepository.UpdateAsync(id, car);
    }

    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}

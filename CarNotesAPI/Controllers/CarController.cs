using CarNotesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private CarRepository _carRepository;

    public CarController(CarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    // GET: api/values
    [HttpGet]
    public async Task<int> GetAsync()
    {
        return await _carRepository.GetCarCount();
    }

    // GET api/values/5
    [HttpGet("{username}")]
    public async Task<List<Car>> Get(string username)
    {
        return await _carRepository.SearchCarsByUsername(username);
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

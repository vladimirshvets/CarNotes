﻿using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly CarRepository _carRepository;

    // ToDo:
    // get from storage when a user functionality is ready.
    private readonly Guid UserId =
        new Guid("48898106-b1b2-4bc1-8f63-3ee777fdeafd");

    public CarsController(CarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    [HttpGet]
    [Route("getByUser")]
    public async Task<IEnumerable<Car>> GetByUser()
    {
        return await _carRepository.GetByUser(UserId);
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisCachingWithNet8.Entities;

namespace RedisCachingWithNet8.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController(ILogger<CarsController> logger) : ControllerBase
{
    [HttpPost("[action]")]
    public IActionResult Create([FromBody] Car car)
    {
        using var context = new CarContext();
        
        context.Database.EnsureCreated();
        
        context.Cars.Add(car);
        
        context.SaveChanges();
        
        return Ok();
    }

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {
        using var context = new CarContext();
        
        var cars = context.Cars.ToList();
        
        return Ok(cars);
    }
}

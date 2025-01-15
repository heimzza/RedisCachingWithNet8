using Microsoft.AspNetCore.Mvc;
using RedisCachingWithNet8.Entities;
using RedisCachingWithNet8.Services.Caching;

namespace RedisCachingWithNet8.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly IRedisCacheService _cache;

    public CarsController(IRedisCacheService cache)
    {
        _cache = cache;
    }
    
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
        var cars = _cache.GetData<IEnumerable<Car>>("cars");

        if (cars is not null)
        {
            return Ok(cars);
        }   
        
        using var context = new CarContext();
        
        cars = context.Cars.ToList();
        
        _cache.SetData<IEnumerable<Car>>("cars", cars);
        return Ok(cars);
    }
}

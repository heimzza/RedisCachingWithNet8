using Microsoft.AspNetCore.Mvc;
using RedisCachingWithNet8.Entities;
using RedisCachingWithNet8.Services.Caching;

namespace RedisCachingWithNet8.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CarsController : ControllerBase
{
    private readonly IRedisCacheService _cache;

    public CarsController(IRedisCacheService cache)
    {
        _cache = cache;
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] Car car)
    {
        using var context = new CarContext();
        
        context.Database.EnsureCreated();
        
        context.Cars.Add(car);
        
        context.SaveChanges();
        
        return Ok();
    }

    [HttpGet]
    public IActionResult GetCarById(int id)
    {
        var car = _cache.GetData<Car>($"car_{id}");
        
        if (car is not null)
        {
            return Ok(car);
        }
        else
        {
            car = _cache.GetData<IEnumerable<Car>>("cars")?
                .FirstOrDefault(x => x.Id == id);
            
            _cache.SetData($"car_{id}", car);
        }
        
        if (car is not null)
        {
            return Ok(car);
        }
        
        using var context = new CarContext();
        
        car = context.Cars.FirstOrDefault(q => q.Id == id);

        if (car is not null)
        {
            _cache.SetData($"car_{id}", car);
        }
        
        return car is not null
            ? Ok(car) 
            : NotFound();
    }

    [HttpGet]
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

using Microsoft.AspNetCore.Mvc;
using RedisCachingWithNet8.Entities;
using RedisCachingWithNet8.Services.Caching;
using RedisCachingWithNet8.Services.Db;

namespace RedisCachingWithNet8.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly IRedisCacheService _cache;

    public CarsController(ICarService carService, IRedisCacheService cache)
    {
        _carService = carService;
        _cache = cache;
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] Car car)
    {
        _carService.Create(car);
        
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
        
        car = _carService.GetById(id);

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
        
        cars = _carService.GetAll();
        
        _cache.SetData<IEnumerable<Car>>("cars", cars);
        
        return Ok(cars);
    }
}

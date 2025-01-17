using RedisCachingWithNet8.Entities;

namespace RedisCachingWithNet8.Services.Db;

public class CarService : ICarService
{
    private readonly CarContext _carContext;

    public CarService(CarContext carContext)
    {
        _carContext = carContext;
        _carContext.Database.EnsureCreated();
    }
    
    public void Create(Car car)
    {
        _carContext.Add(car);
        _carContext.SaveChanges();
    }

    public List<Car> GetAll()
    {
        return _carContext.Cars.ToList();
    }

    public Car? GetById(int id)
    {
        return _carContext.Cars.Find(id);
    }
}
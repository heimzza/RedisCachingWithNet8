using RedisCachingWithNet8.Entities;

namespace RedisCachingWithNet8.Services.Db;

public interface ICarService
{
    void Create(Car car);
    List<Car> GetAll();
    Car? GetById(int id);
}
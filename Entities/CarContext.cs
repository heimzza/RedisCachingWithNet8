using Microsoft.EntityFrameworkCore;

namespace RedisCachingWithNet8.Entities;

public class CarContext: DbContext
{
    public DbSet<Car> Cars { get; set; }
    private string DbPath { get; }

    public CarContext()
    {
        DbPath = "cars.db";
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}

public class Car(string brand, string model, string color, int year)
{
    public string Brand { get; set; } = brand;
    public string Model { get; set; } = model;
    public string Color { get; set; } = color;
    public int Year { get; set; } = year;
}
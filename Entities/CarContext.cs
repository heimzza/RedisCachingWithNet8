using Microsoft.EntityFrameworkCore;

namespace RedisCachingWithNet8.Entities;

public class CarContext: DbContext
{
    public DbSet<Car> Cars { get; set; }
    private static string DbPath => "cars.db";

    public CarContext(DbContextOptions<CarContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}

public class Car(string brand, string model, string color, int year)
{
    public int Id { get; set; }
    public string Brand { get; set; } = brand;
    public string Model { get; set; } = model;
    public string Color { get; set; } = color;
    public int Year { get; set; } = year;
}
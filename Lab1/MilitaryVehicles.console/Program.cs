using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.common;
using MilitaryVehicles.infrastructure;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        //Створюємо контекст бази даних та репозиторій
        var options = new DbContextOptionsBuilder<MilitaryVehiclesContext>()
            .UseSqlite("Data Source=MilitaryVehiclesDb.sqlite")
            .Options;

        using var context = new MilitaryVehiclesContext(options);
        var repository = new Repository<MilitaryVehicle>(context);
        var crudServiceAsync = new CrudServiceAsync<MilitaryVehicle>(repository);

        int countPerType = 10;
        var tasks = new List<Task>();
        ConcurrentBag<MilitaryVehicle> generatedVehicles = new();

        //Створення танків
        tasks.Add(Task.Run(() =>
        {
            Parallel.For(0, countPerType, _ =>
            {
                var tank = Tank.CreateRandom();
                generatedVehicles.Add(tank);
            });
        }));

        // Створення вертольотів
        tasks.Add(Task.Run(() =>
        {
            Parallel.For(0, countPerType, _ =>
            {
                var helicopter = Helicopter.CreateRandom();
                generatedVehicles.Add(helicopter);
            });
        }));

        //Створення есмінців
        tasks.Add(Task.Run(() =>
        {
            Parallel.For(0, countPerType, _ =>
            {
                var destroyer = Destroyer.CreateRandom();
                generatedVehicles.Add(destroyer);
            });
        }));

        await Task.WhenAll(tasks);

        //Додаємо всі об'єкти у сервіс (через репозиторій)
        foreach (var vehicle in generatedVehicles)
        {
            await crudServiceAsync.CreateAsync(vehicle);
        }

        Console.WriteLine("\nСтворено об'єктів: " + generatedVehicles.Count);

        //Отримуємо всі об'єкти
        var allVehicles = await crudServiceAsync.ReadAllAsync();

        var tanks = allVehicles.OfType<Tank>().ToList();
        var helicopters = allVehicles.OfType<Helicopter>().ToList();
        var destroyers = allVehicles.OfType<Destroyer>().ToList();

        Console.WriteLine("\nСтатистика:");

        if (tanks.Any())
        {
            Console.WriteLine($"Танки ({tanks.Count}):");
            Console.WriteLine($"Мін. вогнева міць: {tanks.Min(t => t.Firepower)}");
            Console.WriteLine($"Макс. вогнева міць: {tanks.Max(t => t.Firepower)}");
            Console.WriteLine($"Середня вогнева міць: {tanks.Average(t => t.Firepower):F2}");
        }

        if (helicopters.Any())
        {
            Console.WriteLine($"\nВертольоти ({helicopters.Count}):");
            Console.WriteLine($"Мін. швидкість: {helicopters.Min(h => h.Speed)}");
            Console.WriteLine($"Макс. швидкість: {helicopters.Max(h => h.Speed)}");
            Console.WriteLine($"Середня швидкість: {helicopters.Average(h => h.Speed):F2}");
        }

        if (destroyers.Any())
        {
            Console.WriteLine($"\nЕсмінці ({destroyers.Count}):");
            Console.WriteLine($"Мін. к-сть торпед: {destroyers.Min(d => d.Torpedoes)}");
            Console.WriteLine($"Макс. к-сть торпед: {destroyers.Max(d => d.Torpedoes)}");
            Console.WriteLine($"Середня к-сть торпед: {destroyers.Average(d => d.Torpedoes):F2}");
        }

        Console.WriteLine("\nЗбереження у базу даних...");
        await crudServiceAsync.SaveAsync();

        Console.WriteLine("\nНатисніть, щоб завершити програму...");
        Console.ReadLine();
    }
}
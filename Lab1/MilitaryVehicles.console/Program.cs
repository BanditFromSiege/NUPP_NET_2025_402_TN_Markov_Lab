using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.common;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
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
            .UseSqlite("Data Source=MilitaryVehiclesDB.sqlite")
            .Options;

        Console.WriteLine($"Шлях до бази: {Path.GetFullPath("MilitaryVehiclesDB.sqlite")}\n");

        using var context = new MilitaryVehiclesContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // Репозиторій та CRUD-сервіс
        var repository = new Repository<MilitaryVehicleModel>(context);
        var crudServiceAsync = new CrudServiceAsync<MilitaryVehicleModel>(repository);

        var army1 = new ArmyModel { 
            Id = Guid.NewGuid(),
            Name = "1st Division"
        };

        var army2 = new ArmyModel {
            Id = Guid.NewGuid(),
            Name = "2st Division"
        };

        context.Armies.AddRange(army1, army2);

        var crew1 = new CrewMemberModel { 
            Id = Guid.NewGuid(),
            Name = "Jim Morrison",
            Rank = "Sergeant"
        };

        var crew2 = new CrewMemberModel {
            Id = Guid.NewGuid(),
            Name = "Ian Curtis",
            Rank = "Corporal"
        };

        var crew3 = new CrewMemberModel {
            Id = Guid.NewGuid(),
            Name = "John Lennon",
            Rank = "Lieutenant"
        
        };

        var crew4 = new CrewMemberModel {
            Id = Guid.NewGuid(),
            Name = "John Ritchie",
            Rank = "Captain"
        };

        var crew5 = new CrewMemberModel {
            Id = Guid.NewGuid(),
            Name = "James Hendrix",
            Rank = "Major"
        };

        context.CrewMembers.AddRange(crew1, crew2, crew3, crew4, crew5);

        await context.SaveChangesAsync();

        var vehicles = new List<MilitaryVehicleModel>
        {
            new TankModel {
                Id = Guid.NewGuid(),
                Model = "M1 Abrams",
                Firepower = 120,
                ArmyId = army1.Id,
                CrewMembers = new List<CrewMemberModel> { crew1, crew2 }
            },

            new TankModel {
                Id = Guid.NewGuid(),
                Model = "T-72",
                Firepower = 115,
                ArmyId = army1.Id,
                CrewMembers = new List<CrewMemberModel> { crew4 }
            },

            new TankModel {
                Id = Guid.NewGuid(),
                Model = "T-80",
                Firepower = 120,
                ArmyId = army1.Id,
                CrewMembers = new List<CrewMemberModel> { crew5 }
            },

            new HelicopterModel {
                Id = Guid.NewGuid(),
                Model = "AH-64 Apache",
                Speed = 293,
                ArmyId = army1.Id,
                CrewMembers = new List<CrewMemberModel> { crew3, crew1 }
            },

            new HelicopterModel {
                Id = Guid.NewGuid(),
                Model = "Mi-24",
                Speed = 280,
                ArmyId = army1.Id,
                CrewMembers = new List<CrewMemberModel> { crew4 }
            },

            new HelicopterModel {
                Id = Guid.NewGuid(),
                Model = "UH-60 Black Hawk",
                Speed = 295,
                ArmyId = army1.Id,
                CrewMembers = new List<CrewMemberModel> { crew2, crew5 }
            },

            new DestroyerModel {
                Id = Guid.NewGuid(),
                Model = "USS Arleigh Burke",
                Torpedoes = 8,
                ArmyId = army2.Id,
                CrewMembers = new List<CrewMemberModel> { crew4, crew5 }
            },

            new DestroyerModel {
                Id = Guid.NewGuid(),
                Model = "USS Kidd",
                Torpedoes = 8,
                ArmyId = army2.Id,
                CrewMembers = new List<CrewMemberModel> { crew3 }
            },

            new DestroyerModel {
                Id = Guid.NewGuid(),
                Model = "USS John Paul Jones",
                Torpedoes = 6,
                ArmyId = army2.Id,
                CrewMembers = new List<CrewMemberModel> { crew2 }
            },

            new DestroyerModel {
                Id = Guid.NewGuid(),
                Model = "USS Hobart-class",
                Torpedoes = 6,
                ArmyId = army2.Id,
                CrewMembers = new List<CrewMemberModel> { crew1, crew5 }
            }
        };

        foreach (var v in vehicles)
        {
            await crudServiceAsync.CreateAsync(v);
        }

        Console.WriteLine("\nСтворені об'єкти у базі:\n");

        var allVehicles = await context.MilitaryVehicles
            .Include(v => v.Army)
            .Include(v => v.CrewMembers)
            .ToListAsync();

        foreach (var v in allVehicles)
        {
            Console.WriteLine($"Тип: {v.GetType().Name}");
            Console.WriteLine($"Модель: {v.Model}");
            Console.WriteLine($"Армія: {v.Army?.Name}");
            Console.WriteLine($"Екіпаж: {string.Join(", ", v.CrewMembers.Select(c => $"{c.Rank} {c.Name}"))}");

            switch (v)
            {
                case TankModel tank:
                    Console.WriteLine($"Вогнева міць: {tank.Firepower}");
                    break;
                case HelicopterModel heli:
                    Console.WriteLine($"Швидкість: {heli.Speed}");
                    break;
                case DestroyerModel ship:
                    Console.WriteLine($"Торпеди: {ship.Torpedoes}");
                    break;
            }

            Console.WriteLine();
        }

        Console.WriteLine("\nПеревірка стану бази даних:\n");

        int armyCount = await context.Armies.CountAsync();
        int crewCount = await context.CrewMembers.CountAsync();
        int vehicleCount = await context.MilitaryVehicles.CountAsync();
        int tankCount = await context.Tanks.CountAsync();
        int heliCount = await context.Helicopters.CountAsync();
        int destroyerCount = await context.Destroyers.CountAsync();

        int linkCount = 0;
        using (var cmd = context.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = "SELECT COUNT(*) FROM VehicleCrewMembers;";
            await context.Database.OpenConnectionAsync();
            var result = await cmd.ExecuteScalarAsync();
            linkCount = Convert.ToInt32(result);
            await context.Database.CloseConnectionAsync();
        }

        Console.WriteLine($"Армій: {armyCount}");
        Console.WriteLine($"Членів екіпажу: {crewCount}");
        Console.WriteLine($"Всього транспортних засобів: {vehicleCount}");
        Console.WriteLine($"Танків: {tankCount}");
        Console.WriteLine($"Гелікоптерів: {heliCount}");
        Console.WriteLine($"Есмінців: {destroyerCount}");
        Console.WriteLine($"Зв’язків Vehicle_Crew: {linkCount}");

        Console.WriteLine("\nПеревірка завершена!\n");

        Console.WriteLine("\nНатисніть, щоб завершити програму...");
        Console.ReadLine();
    }
}
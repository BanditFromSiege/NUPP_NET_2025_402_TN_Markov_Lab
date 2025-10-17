using MilitaryVehicles.common;
using System.Reflection;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var tank = new Tank("T-72");
        var helicopter = new Helicopter("Mi-24");
        var destroyer = new Destroyer("Project 7");

        var filePath = "vehicles.json";
        var crudServiceAsync = new CrudServiceAsync<MilitaryVehicle>(filePath);

        Console.WriteLine();

        await crudServiceAsync.CreateAsync(tank);
        await crudServiceAsync.CreateAsync(helicopter);
        await crudServiceAsync.CreateAsync(destroyer);

        Console.WriteLine();

        var allVehicles = await crudServiceAsync.ReadAllAsync();
        foreach (var vehicle in allVehicles)
        {
            vehicle.PrintInfo();
        }

        Console.WriteLine();

        var paginatedVehicles = await crudServiceAsync.ReadAllAsync(1, 2);
        foreach (var vehicle in paginatedVehicles)
        {
            vehicle.PrintInfo();
        }

        Console.WriteLine();

        await crudServiceAsync.RemoveAsync(tank);
    }
}
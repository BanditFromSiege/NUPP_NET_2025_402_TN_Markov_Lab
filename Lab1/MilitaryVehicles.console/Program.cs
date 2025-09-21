using MilitaryVehicles.common;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<Tank> SovietTanks = new List<Tank> {
            new Tank("T-72"),
            new Tank("T-64"),
            new Tank("T-80"),
        };

        List<Tank> USTanks = new List<Tank> {
            new Tank("M1 Abrams"),
            new Tank("M60 Patton"),
            new Tank("M4 Sherman"),
        };

        List<Helicopter> SovietHelicopters = new List<Helicopter> {
            new Helicopter("Mi-24"),
            new Helicopter("Mi-8"),
            new Helicopter("Ka-50"),
        };

        List<Helicopter> USHelicopters = new List<Helicopter> {
            new Helicopter("UH-60 Black Hawk"),
            new Helicopter("AH-64 Apache"),
            new Helicopter("CH-47 Chinook"),
        };

        List<Destroyer> SovietDestroyers = new List<Destroyer> {
            new Destroyer("Project 7"),
            new Destroyer("Project 956"),
            new Destroyer("Project 1155"),
        };

        List<Destroyer> USDestroyers = new List<Destroyer> {
            new Destroyer("USS Arleigh Burke"),
            new Destroyer("USS Kidd"),
            new Destroyer("USS John Paul Jones"),
        };

        Console.WriteLine();

        SovietTanks[0].StartEngine();
        SovietHelicopters[0].StartEngine();
        SovietDestroyers[0].StartEngine();

        Console.WriteLine();

        SovietTanks[0].Fire();
        SovietHelicopters[0].TakeOff();
        SovietDestroyers[0].FireGuns();

        Console.WriteLine();

        var CrudService = new CrudService<MilitaryVehicle>();

        foreach (var tank in SovietTanks)
        {
            CrudService.Create(tank);
        }
        Console.WriteLine();

        foreach (var tank in USTanks)
        {
            CrudService.Create(tank);
        }
        Console.WriteLine();

        foreach (var chopper in SovietHelicopters)
        {
            CrudService.Create(chopper);
        }
        Console.WriteLine();

        foreach (var chopper in USHelicopters)
        {
            CrudService.Create(chopper);
        }
        Console.WriteLine();

        foreach (var destroyer in SovietDestroyers)
        {
            CrudService.Create(destroyer);
        }
        Console.WriteLine();

        foreach (var destroyer in USDestroyers)
        {
            CrudService.Create(destroyer);
        }
        Console.WriteLine();

        try
        {
            var foundTank = CrudService.Read(SovietTanks[0].Id);
            Console.WriteLine($"Знайдено танк: {foundTank.Model} з ідентифікатором {foundTank.Id}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        CrudService.Remove(SovietTanks[0]);

        SovietTanks[1].Model = "T-34";
        CrudService.Update(SovietTanks[1]);

        Console.WriteLine("\nПерелік всієї техніки:");
        int i = 0;
        foreach (var el in CrudService.ReadAll())
        {
            Console.WriteLine($"{el.GetType().Name}: {el.Model}, ідентифікатор: {el.Id}");
            ++i;
            if (i % 3 == 0)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine("\nНатисніть, щоб завершити програму...");
        Console.ReadLine();
    }
}
using MilitaryVehicles.common;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void DemoLock()
    {
        object locker = new object();
        int counter = 0;

        Parallel.For(0, 1000, _ =>
        {
            lock (locker)
            {
                counter++;
            }
        });

        Console.WriteLine($"[lock] Значення лічильника: {counter}");
    }

    static async Task DemoSemaphoreAsync()
    {
        var semaphore = new SemaphoreSlim(3);
        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            int localI = i;
            tasks.Add(Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                Console.WriteLine($"[Semaphore] Завдання {localI} розпочато");
                await Task.Delay(100);
                Console.WriteLine($"[Semaphore] Завдання {localI} завершено");
                semaphore.Release();
            }));
        }

        await Task.WhenAll(tasks);
    }

    static void DemoMonitor()
    {
        object monitorLock = new object();
        int shared = 0;

        Parallel.For(0, 1000, _ =>
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(monitorLock, ref lockTaken);
                shared++;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(monitorLock);
                }
            }
        });

        Console.WriteLine($"[Monitor] Спільне значення: {shared}");
    }

    static void DemoMutex()
    {
        using var mutex = new Mutex();

        Parallel.For(0, 100, i =>
        {
            mutex.WaitOne();
            Console.WriteLine($"[Mutex] Потік {i} увійшов у критичну секцію");
            Thread.Sleep(10);
            Console.WriteLine($"[Mutex] Потік {i} виходить із критичної секції");
            mutex.ReleaseMutex();
        });
    }

    static void DemoAutoResetEvent()
    {
        var autoResetEvent = new AutoResetEvent(false);

        Task.Run(() =>
        {
            Console.WriteLine("[AutoResetEvent] Очікування сигналу...");
            autoResetEvent.WaitOne();
            Console.WriteLine("[AutoResetEvent] Сигнал отримано!");
        });

        Thread.Sleep(500);
        Console.WriteLine("[AutoResetEvent] Надсилання сигналу...");
        autoResetEvent.Set();
    }

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var filePath = "vehicles.json";
        var crudServiceAsync = new CrudServiceAsync<MilitaryVehicle>(filePath);

        int countPerType = 500;

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

        //Створення вертольотів
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

        //Дочекатися створення всіх об'єктів
        await Task.WhenAll(tasks);

        //Додати всі об'єкти у сервіс
        foreach (var vehicle in generatedVehicles)
        {
            await crudServiceAsync.CreateAsync(vehicle);
        }

        Console.WriteLine("\nСтворено об'єктів: " + generatedVehicles.Count);

        //Рахуємо статистику
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

        Console.WriteLine("\nЗбереження у файл...");
        await crudServiceAsync.SaveAsync();

        Console.WriteLine("\nНатисніть, щоб перейти до прикладу користування Lock...");
        Console.ReadLine();

        DemoLock();

        Console.WriteLine("\nНатисніть, щоб перейти до прикладу користування Semaphore...");
        Console.ReadLine();

        await DemoSemaphoreAsync();

        Console.WriteLine("\nНатисніть, щоб перейти до прикладу користування Monitor...");
        Console.ReadLine();

        DemoMonitor();

        Console.WriteLine("\nНатисніть, щоб перейти до прикладу користування Mutex...");
        Console.ReadLine();

        DemoMutex();

        Console.WriteLine("\nНатисніть, щоб перейти до прикладу користування AutoResetEvent...");
        Console.ReadLine();

        DemoAutoResetEvent();

        Console.WriteLine("\nНатисніть, щоб завершити програму...");
        Console.ReadLine();
    }
}
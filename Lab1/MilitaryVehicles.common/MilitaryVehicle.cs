using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public abstract class MilitaryVehicle
    {
        public Guid Id { get; set; } //Унікальний ідентифікатор
        public string Model { get; set; } //Модель транспортного засобу

        private static uint vehicleCount = 0; //Статичне поле для підрахунку створених ТС

        // Делегат для події запуску двигуна
        public delegate void EngineStartedHandler(MilitaryVehicle sender);

        public event EngineStartedHandler? EngineStarted; //Подія запуску двигуна

        //Статичний конструктор
        static MilitaryVehicle()
        {
            Console.WriteLine("Статичний конструктор. Створено новий клас MilitaryVehicle");
        }

        //Конструктор
        public MilitaryVehicle(string model)
        {
            Id = Guid.NewGuid();
            Model = model;
            vehicleCount++;
        }

        //Статичний метод для отримання кількості створених транспортних засобів
        public static uint GetVehicleCount()
        {
            return vehicleCount;
        }

        //Абстрактний метод для запуску двигуна
        public abstract void StartEngine();

        //Метод для виклику події
        protected void OnEngineStarted()
        {
            EngineStarted?.Invoke(this);
        }
    }
}
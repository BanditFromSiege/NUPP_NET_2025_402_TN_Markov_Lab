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

        //Статичний конструктор
        static MilitaryVehicle()
        {
            Console.WriteLine("Створено новий клас MilitaryVehicle");
        }

        //Конструктор
        public MilitaryVehicle(string model)
        {
            Id = Guid.NewGuid();
            Model = model;
        }

        //Абстрактний метод для запуску двигуна
        public abstract void StartEngine();
    }
}
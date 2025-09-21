using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    //Клас для морських транспортних засобів
    public abstract class SeaVehicle : MilitaryVehicle
    {
        //Статичний конструктор
        static SeaVehicle()
        {
            Console.WriteLine("Статичний конструктор. Створено новий клас SeaVehicle");
        }

        //Конструктор
        public SeaVehicle(string model) : base(model) {}

        //Перевизначення методу для запуску двигуна
        public override void StartEngine()
        {
            Console.WriteLine($"Двигун морського транспортного засобу {Model} з ідентифікатором {Id} заведено");
        }
    }
}
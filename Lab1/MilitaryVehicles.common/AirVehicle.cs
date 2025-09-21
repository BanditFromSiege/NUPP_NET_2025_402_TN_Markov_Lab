using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    //Клас для повітряних транспортних засобів
    public abstract class AirVehicle : MilitaryVehicle
    {
        //Статичний конструктор
        static AirVehicle()
        {
            Console.WriteLine("Створено новий клас AirVehicle");
        }

        //Конструктор
        public AirVehicle(string model) : base(model) {}

        //Перевизначення методу для запуску двигуна
        public override void StartEngine()
        {
            Console.WriteLine($"Двигун повітряного транспортного засобу {Model} з ідентифікатором {Id} заведено");
        }
    }
}
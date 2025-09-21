using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    //Клас для наземних транспортних засобів
    public abstract class GroundVehicle : MilitaryVehicle
    {
        //Статичний конструктор
        static GroundVehicle()
        {
            Console.WriteLine("Статичний конструктор. Створено новий клас GroundVehicle");
        }

        //Конструктор
        public GroundVehicle(string model) : base(model) {}

        //Перевизначення методу для запуску двигуна
        public override void StartEngine()
        {
            Console.WriteLine($"Двигун наземного транспортного засобу {Model} з ідентифікатором {Id} заведено");
        }
    }
}
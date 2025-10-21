using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class Helicopter : AirVehicle
    {
        //Конструктор
        public Helicopter(string model) : base(model) {}

        //Метод для зльоту
        public void TakeOff()
        {
            Console.WriteLine($"Вертоліт {Model} з ідентифікатором {Id} злетів");
        }
    }
}
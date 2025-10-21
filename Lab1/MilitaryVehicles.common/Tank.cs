using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class Tank : GroundVehicle
    {
        //Конструктор
        public Tank(string model) : base(model) {}

        //Метод для стрільби
        public void Fire()
        {
            Console.WriteLine($"Танк {Model} з ідентифікатором {Id} відкрив вогонь");
        }
    }
}
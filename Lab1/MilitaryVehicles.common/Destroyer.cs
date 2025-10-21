using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class Destroyer : SeaVehicle
    {
        //Конструктор
        public Destroyer(string model) : base(model) {}

        //Метод для стрільби гарматами
        public void FireGuns()
        {
            Console.WriteLine($"Есмінець {Model} з ідентифікатором {Id} відкрив вогонь із гармат");
        }
    }
}
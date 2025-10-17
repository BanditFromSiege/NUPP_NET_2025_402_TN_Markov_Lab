using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class Tank : GroundVehicle
    {
        public int Firepower { get; private set; }

        //Конструктор
        public Tank(string model, int firepower = 100) : base(model)
        {
            Firepower = firepower;
        }

        //Метод для стрільби
        public void Fire()
        {
            Console.WriteLine($"Танк {Model} з ідентифікатором {Id} відкрив вогонь");
        }

        //Статичний метод
        public static Tank CreateRandom()
        {
            var rnd = new Random();
            string model = $"Tank-{Guid.NewGuid().ToString().Substring(0, 4)}";
            int firepower = rnd.Next(50, 151);
            return new Tank(model, firepower);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class Helicopter : AirVehicle
    {
        public int Speed { get; set; }
        public string Type => "Helicopter";

        //Конструткор за замовчуванням
        public Helicopter() : base("Unknown")
        {
            Speed = 0;
        }

        //Конструктор
        public Helicopter(string model, int speed = 275) : base(model)
        {
            Speed = speed;
        }

        //Метод для зльоту
        public void TakeOff()
        {
            Console.WriteLine($"Вертоліт {Model} з ідентифікатором {Id} злетів");
        }

        //Статичний метод
        public static Helicopter CreateRandom()
        {
            var rnd = new Random();
            string model = $"Helicopter-{Guid.NewGuid().ToString().Substring(0, 4)}";
            int speed = rnd.Next(200, 351);
            return new Helicopter(model, speed);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class Destroyer : SeaVehicle
    {
        public int Torpedoes { get; private set; }

        //Конструктор
        public Destroyer(string model, int torpedoes = 12) : base(model)
        {
            Torpedoes = torpedoes;
        }

        //Метод для стрільби гарматами
        public void FireGuns()
        {
            Console.WriteLine($"Есмінець {Model} з ідентифікатором {Id} відкрив вогонь із гармат");
        }

        //Статичний метод
        public static Destroyer CreateRandom()
        {
            var rnd = new Random();
            string model = $"Destroyer-{Guid.NewGuid().ToString().Substring(0, 4)}";
            int torpedoes = rnd.Next(4, 21);
            return new Destroyer(model, torpedoes);
        }
    }
}
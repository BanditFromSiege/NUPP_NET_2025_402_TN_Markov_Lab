using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public static class VehicleExtensions
    {
        public static void PrintInfo(this MilitaryVehicle vehicle)
        {
            Console.WriteLine($"{vehicle.GetType().Name}: {vehicle.Model}. Ідентифікатор: {vehicle.Id}");
        }
    }
}
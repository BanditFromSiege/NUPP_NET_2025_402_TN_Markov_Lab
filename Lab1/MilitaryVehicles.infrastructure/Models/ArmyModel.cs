using System;
using System.Collections.Generic;

namespace MilitaryVehicles.infrastructure.Models
{
    //Модель армії (для зв’язку один-до-багатьох)
    public class ArmyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        //Один-до-багатьох — армія має кілька транспортних засобів
        public ICollection<MilitaryVehicleModel> Vehicles { get; set; } = new List<MilitaryVehicleModel>();
    }
}
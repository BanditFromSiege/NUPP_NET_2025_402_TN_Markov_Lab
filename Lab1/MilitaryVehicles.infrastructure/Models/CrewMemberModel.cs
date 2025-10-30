using System;
using System.Collections.Generic;

namespace MilitaryVehicles.infrastructure.Models
{
    //Модель члена екіпажу (для зв’язку "багато-до-багатьох")
    public class CrewMemberModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;

        //Багато-до-багатьох — член екіпажу може працювати на кількох машинах
        public ICollection<MilitaryVehicleModel> Vehicles { get; set; } = new List<MilitaryVehicleModel>();
    }
}
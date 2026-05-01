using System;
using System.Collections.Generic;

namespace MilitaryVehicles.infrastructure.Models
{
    //Базова таблиця — MilitaryVehicle
    public abstract class MilitaryVehicleModel
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;

        //Один-до-багатьох: кожен транспорт належить одній армії
        public Guid ArmyId { get; set; }
        public ArmyModel Army { get; set; } = null!;

        //Багато-до-багатьох: транспорт має екіпаж
        public ICollection<CrewMemberModel> CrewMembers { get; set; } = new List<CrewMemberModel>();
    }
}
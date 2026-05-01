namespace MilitaryVehicles.REST.Models
{
    public class TankCreateModel
    {
        public string Model { get; set; } = string.Empty;
        public int Firepower { get; set; }
        public Guid ArmyId { get; set; }
    }

    public class TankUpdateModel
    {
        public string Model { get; set; } = string.Empty;
        public int Firepower { get; set; }
    }

    public class TankResponseModel
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Firepower { get; set; }

        public Guid ArmyId { get; set; }
        public List<Guid> CrewMemberIds { get; set; } = new();
    }
}
namespace MilitaryVehicles.REST.Models
{
    public class HelicopterCreateModel
    {
        public string Model { get; set; } = string.Empty;
        public int Speed { get; set; }
        public Guid ArmyId { get; set; }
        public List<Guid> CrewMemberIds { get; set; } = new();
    }

    public class HelicopterUpdateModel
    {
        public string? Model { get; set; }
        public int? Speed { get; set; }
        public Guid? ArmyId { get; set; }
        public List<Guid>? CrewMemberIds { get; set; }
    }

    public class HelicopterResponseModel
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Speed { get; set; }
        public Guid ArmyId { get; set; }
        public List<Guid> CrewMemberIds { get; set; } = new();
    }
}
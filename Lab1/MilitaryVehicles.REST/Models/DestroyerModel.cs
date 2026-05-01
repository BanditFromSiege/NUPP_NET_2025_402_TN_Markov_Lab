namespace MilitaryVehicles.REST.Models
{
    public class DestroyerCreateModel
    {
        public string Model { get; set; } = string.Empty;
        public int Torpedoes { get; set; }
        public Guid ArmyId { get; set; }
        public List<Guid> CrewMemberIds { get; set; } = new();
    }

    public class DestroyerUpdateModel
    {
        public string? Model { get; set; }
        public int? Torpedoes { get; set; }
        public Guid? ArmyId { get; set; }
        public List<Guid>? CrewMemberIds { get; set; }
    }

    public class DestroyerResponseModel
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Torpedoes { get; set; }
        public Guid ArmyId { get; set; }

        public List<Guid> CrewMemberIds { get; set; } = new();
    }
}

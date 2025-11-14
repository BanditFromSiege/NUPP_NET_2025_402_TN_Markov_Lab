namespace MilitaryVehicles.REST.Models
{
    public class CrewMemberCreateModel
    {
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
    }

    public class CrewMemberUpdateModel
    {
        public string? Name { get; set; }
        public string? Rank { get; set; }
    }

    public class CrewMemberResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;

        public List<Guid> VehicleIds { get; set; } = new();
    }
}

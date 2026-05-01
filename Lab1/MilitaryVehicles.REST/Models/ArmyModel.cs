namespace MilitaryVehicles.REST.Models
{
    public class ArmyCreateModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ArmyUpdateModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ArmyResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Guid> VehicleIds { get; set; } = new();
    }
}

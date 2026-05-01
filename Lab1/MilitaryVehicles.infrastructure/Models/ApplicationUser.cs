using Microsoft.AspNetCore.Identity;

namespace MilitaryVehicles.infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
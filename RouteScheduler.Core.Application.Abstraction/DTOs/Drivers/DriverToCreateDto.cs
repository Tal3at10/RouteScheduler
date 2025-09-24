using System.ComponentModel.DataAnnotations;

namespace RouteScheduler.Core.Application.Abstraction.DTOs.Drivers
{
    public class DriverToCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string LicenseNumber { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
    }
}


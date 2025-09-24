using System.ComponentModel.DataAnnotations;

namespace RouteScheduler.Core.Application.Abstraction.DTOs.Routes
{
    public class RouteToUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Origin { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Destination { get; set; } = string.Empty;

        [Range(0.1, double.MaxValue, ErrorMessage = "Distance must be greater than 0")]
        public double Distance { get; set; }

        [Required]
        public TimeSpan EstimatedTime { get; set; }
    }
}

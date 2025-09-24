using System.ComponentModel.DataAnnotations;

namespace RouteScheduler.Core.Application.Abstraction.DTOs.Scheduals
{
    public class ScheduleToCreateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "DriverId must be greater than 0")]
        public int DriverId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "RouteId must be greater than 0")]
        public int RouteId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Scheduled";
    }
}

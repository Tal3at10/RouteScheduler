namespace RouteScheduler.Core.Application.Abstraction.DTOs.Scheduals
{
    public class ScheduleToReturnDto
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int RouteId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        
        // Navigation properties for display
        public string DriverName { get; set; } = string.Empty;
        public string RouteOrigin { get; set; } = string.Empty;
        public string RouteDestination { get; set; } = string.Empty;
    }
}

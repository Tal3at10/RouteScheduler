namespace RouteScheduler.Core.Application.Abstraction.DTOs.Routes
{
    public class RouteToReturnDto
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public double Distance { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}

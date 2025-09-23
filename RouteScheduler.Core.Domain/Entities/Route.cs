using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteScheduler.Core.Domain.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public required string Origin { get; set; } 
        public required string Destination { get; set; } 
        public double Distance { get; set; }
        public TimeSpan EstimatedTime { get; set; }

        // Navigation
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}

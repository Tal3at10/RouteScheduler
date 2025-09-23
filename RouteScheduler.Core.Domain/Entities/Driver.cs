using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteScheduler.Core.Domain.Entities
{
    public class Driver : BaseAuditableEntity<int>  
    {
        public required string Name { get; set; }
        public required string LicenseNumber { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}

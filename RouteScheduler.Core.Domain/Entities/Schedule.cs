using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteScheduler.Core.Domain.Entities
{
    public class Schedule : BaseAuditableEntity<int>    
    {
        public int DriverId { get; set; }
        public int RouteId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        //// Navigation
        public Driver Driver { get; set; } = null!;
        public Route Route { get; set; } = null!;
    }
}

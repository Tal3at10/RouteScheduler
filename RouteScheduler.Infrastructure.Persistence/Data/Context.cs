using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RouteScheduler.Core.Domain.Entities;

namespace RouteScheduler.Infrastructure.Persistence.Data
{
    public class Context : Microsoft.EntityFrameworkCore.DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This line is important to call the base method which is in DbContext class

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformations).Assembly); // using the AssemblyInformations class to get the assembly
        }
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;

    }
}

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

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity<int>>()
                         .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "System"; 
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = "System"; 
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;

    }
}

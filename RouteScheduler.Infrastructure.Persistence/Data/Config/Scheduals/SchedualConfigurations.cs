using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RouteScheduler.Core.Domain.Entities;
using RouteScheduler.Infrastructure.Persistence.Data.Config.BaseAuditableEntity;

namespace RouteScheduler.Infrastructure.Persistence.Data.Config.Schedules
{
    public class ScheduleConfiguration : BaseAuditableEntityConfiguration<Schedule, int>
    {
        public override void Configure(EntityTypeBuilder<Schedule> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.Date)
                   .IsRequired();

            builder.Property(s => s.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Scheduled");

            // Relationships
            builder.HasOne(s => s.Driver)
                   .WithMany(d => d.Schedules)
                   .HasForeignKey(s => s.DriverId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Route)
                   .WithMany(r => r.Schedules)
                   .HasForeignKey(s => s.RouteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

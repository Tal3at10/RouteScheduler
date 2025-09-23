using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RouteScheduler.Core.Domain.Entities;
using RouteScheduler.Infrastructure.Persistence.Data.Config.BaseAuditableEntity;

namespace RouteScheduler.Infrastructure.Persistence.Data.Config.Routes
{
    public class RouteConfiguration : BaseAuditableEntityConfiguration<Route, int>
    {
        public override void Configure(EntityTypeBuilder<Route> builder)
        {
            base.Configure(builder); // Call base cuz it has common configurations which like CreatedBy, CreatedAt, in the BaseAuditableEntityConfiguration class 

            builder.Property(r => r.Origin)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(r => r.Destination)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(r => r.Distance)
                   .IsRequired();

            builder.Property(r => r.EstimatedTime)
                   .IsRequired();

            // Relationships
            builder.HasMany(r => r.Schedules)
                   .WithOne(s => s.Route)
                   .HasForeignKey(s => s.RouteId);
        }
    }
}

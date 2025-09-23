using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RouteScheduler.Core.Domain.Entities;
using RouteScheduler.Infrastructure.Persistence.Data.Config.BaseAuditableEntity;

public class DriverConfiguration : BaseAuditableEntityConfiguration<Driver, int>
{
    public override void Configure(EntityTypeBuilder<Driver> builder)
    {
        base.Configure(builder); 

        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(d => d.LicenseNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(d => d.IsAvailable)
               .IsRequired();

        builder.HasIndex(d => d.LicenseNumber)
               .IsUnique();

        builder.HasMany(d => d.Schedules)
               .WithOne(s => s.Driver)
               .HasForeignKey(s => s.DriverId);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RouteScheduler.Infrastructure.Persistence.Data.Config.BaseAuditableEntity
{
    public abstract class BaseAuditableEntityConfiguration<TEntity, TKey>
     : IEntityTypeConfiguration<TEntity>
     where TEntity : BaseAuditableEntity<TKey>
     where TKey : IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedBy).HasMaxLength(100);
            builder.Property(e => e.UpdatedAt).IsRequired(false);
        }
    }

}

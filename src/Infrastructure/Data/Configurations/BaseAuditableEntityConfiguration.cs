using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BaseAuditableEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
    where TEntity : BaseAuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(e => e.DeletedAt)
            .HasDefaultValue(null);

        builder.HasQueryFilter(e => e.DeletedAt == null);
    }
}
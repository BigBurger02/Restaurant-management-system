using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class DishInMenuConfiguration : IEntityTypeConfiguration<DishInMenuEntity>
{
    public void Configure(EntityTypeBuilder<DishInMenuEntity> builder)
    {
        builder.HasKey(k => k.ID);

        builder.Property(d => d.Name)
            .HasColumnType("nvarchar(50)")
            .IsRequired(true);

        builder.ToTable("DishesInMenu");
    }
}
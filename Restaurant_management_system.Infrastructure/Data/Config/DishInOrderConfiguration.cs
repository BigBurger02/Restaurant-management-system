using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class DishInOrderConfiguration : IEntityTypeConfiguration<DishInOrderEntity>
{
    public void Configure(EntityTypeBuilder<DishInOrderEntity> builder)
    {
        builder.HasKey(k => k.ID);

        builder.Property(d => d.OrderID)
            .HasColumnType("int");
        builder.Property(d => d.DishName)
            .HasColumnType("nvarchar(50)")
            .IsRequired(false);
        builder.Property(d => d.DateOfOrdering)
            .HasColumnType("datetime")
            .IsRequired(true);
        builder.Property(d => d.IsDone)
            .HasColumnType("bit");
        builder.Property(d => d.IsTakenAway)
            .HasColumnType("bit");
        builder.Property(d => d.IsPrioritized)
            .HasColumnType("bit");

        builder.ToTable("DishesInOrder");
    }
}
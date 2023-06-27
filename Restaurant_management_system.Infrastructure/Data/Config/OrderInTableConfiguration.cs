using Restaurant_management_system.Core.TablesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class OrderInTableConfiguration : IEntityTypeConfiguration<OrderInTableEntity>
{
    public void Configure(EntityTypeBuilder<OrderInTableEntity> builder)
    {
        builder.HasKey(k => k.ID);
        builder.Ignore(o => o.Dishes);

        builder.Property(d => d.TableID)
            .HasColumnType("int");
        builder.Property(d => d.Open)
            .HasColumnType("bit");
        builder.Property(d => d.SelfOrdered)
            .HasColumnType("bit");
        builder.Property(d => d.Message)
            .HasColumnType("nvarchar(500)")
            .IsRequired(false);

        builder.ToTable("OrdersInTable");
    }
}
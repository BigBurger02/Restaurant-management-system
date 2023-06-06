using Restaurant_management_system.Core.TablesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class TableConfiguration : IEntityTypeConfiguration<TableEntity>
{
    public void Configure(EntityTypeBuilder<TableEntity> builder)
    {
        builder.Ignore(t => t.Order);


        builder.Property(t => t.IsOccupied)
            .HasColumnType("bit");

        builder.Property(t => t.IsPaid)
            .HasColumnType("bit");

        builder.Property(d => d.AmountOfGuests)
            .HasColumnType("int");

        builder.Property(d => d.OrderCost)
            .HasColumnType("int");


        builder.ToTable("Tables");
    }
}
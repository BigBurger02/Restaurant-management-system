using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class IngredientConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder.HasKey(k => k.ID);

        builder.Property(d => d.Name)
            .HasColumnType("nvarchar(50)")
            .IsRequired(false);
        builder.Property(d => d.Price)
            .HasColumnType("int");

        builder.ToTable("Ingredients");
    }
}
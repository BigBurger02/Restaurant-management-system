using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class IngredientForDishInMenuConfiguration : IEntityTypeConfiguration<IngredientForDishInMenuEntity>
{
    public void Configure(EntityTypeBuilder<IngredientForDishInMenuEntity> builder)
    {
        builder.HasKey(k => k.ID);

        builder.Property(d => d.DishInMenuID)
            .HasColumnType("int");
        builder.Property(d => d.IngredientID)
            .HasColumnType("int");

        builder.ToTable("IngredientsForDishInMenu");
    }
}
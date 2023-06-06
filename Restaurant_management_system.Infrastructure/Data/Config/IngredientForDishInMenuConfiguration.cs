using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_management_system.Infrastructure.Data.Config;

public class IngredientForDishInMenuConfiguration : IEntityTypeConfiguration<IngredientForDishInMenuEntity>
{
    public void Configure(EntityTypeBuilder<IngredientForDishInMenuEntity> builder)
    {
        builder.Property(d => d.MenuID)
            .HasColumnType("int");

        builder.Property(d => d.IngredientID)
            .HasColumnType("int");


        builder.ToTable("IngredientsForDishInMenu");
    }
}
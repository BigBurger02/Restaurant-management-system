using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_management_system.Infrastructure.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }

        public DbSet<DishEntity> Dish { get; set; }
        public DbSet<TableEntity> Table { get; set; }
        public DbSet<IngredientEntity> Ingredient { get; set; }
        public DbSet<MenuEntity> Menu { get; set; }
        public DbSet<OrderEntity> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishEntity>().ToTable("Dishes");
            modelBuilder.Entity<TableEntity>().ToTable("Tables");
            modelBuilder.Entity<IngredientEntity>().ToTable("Ingredients");
            modelBuilder.Entity<MenuEntity>().ToTable("Menu");
            modelBuilder.Entity<OrderEntity>().ToTable("Orders");
        }
    }
}
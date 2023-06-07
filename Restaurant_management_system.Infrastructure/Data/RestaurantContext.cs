using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Restaurant_management_system.Infrastructure.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }

        public DbSet<DishInOrderEntity> DishInOrder { get; set; }
        public DbSet<TableEntity> Table { get; set; }
        public DbSet<IngredientEntity> Ingredient { get; set; }
        public DbSet<DishInMenuEntity> DishInMenu { get; set; }
        public DbSet<OrderInTableEntity> OrderInTable { get; set; }
        public DbSet<IngredientForDishInMenuEntity> IngredientForDishInMenu { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
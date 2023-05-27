using Restaurant_management_system.Core.DishesAggregate;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_management_system.Infrastructure.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }

        public DbSet<DishEntity> Dish { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishEntity>().ToTable("Dishes");
        }
    }
}
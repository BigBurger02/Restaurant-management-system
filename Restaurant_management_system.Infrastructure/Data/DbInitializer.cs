using Restaurant_management_system.Core.DishesAggregate;

namespace Restaurant_management_system.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void Initialize(RestaurantContext context)
        {
            context.Database.EnsureCreated();

            if (context.Dish.Any())
                return;

            var dishes = new DishEntity[]
            {
                new DishEntity{ID=1, DishName="Chicken Strips", DateOfOrdering=new DateTime(2023, 5, 15, 16, 59, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=true, IsTakenAway=true, IsPrioritized=false},
                new DishEntity{ID=2, DishName="French Dip",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=3, DishName="Cobb Salad",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=4, DishName="Meat Loaf",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=true, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=5, DishName="Cannoli",        DateOfOrdering=new DateTime(2023, 5, 15, 20, 26, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=6, DishName="ClubHouse",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 20, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=true, IsTakenAway=true, IsPrioritized=false},
                new DishEntity{ID=7, DishName="Roast Pork",     DateOfOrdering=new DateTime(2023, 5, 15, 19, 40, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=false, IsTakenAway=false, IsPrioritized=true},
                new DishEntity{ID=8, DishName="Roast Beef",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 23, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=true, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=9, DishName="White Pizza",    DateOfOrdering=new DateTime(2023, 5, 15, 20, 25, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=10, DishName="Hamburger",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00).ToString("dd.MM.yyyy HH:mm"), IsDone=false, IsTakenAway=false, IsPrioritized=false},
            };
            foreach (var item in dishes)
                context.Add(item);
            context.SaveChanges();
        }
    }
}


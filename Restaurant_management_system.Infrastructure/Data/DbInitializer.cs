using System.Net.NetworkInformation;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void Initialize(RestaurantContext context)
        {
            context.Database.EnsureCreated();

            if (context.Dish.Any())
                return;

            var ingredients = new IngredientEntity[]
            {
                new IngredientEntity{ ID=1,  Name="Water", Price=15 },
                new IngredientEntity{ ID=2,  Name="Oil", Price=50 },
                new IngredientEntity{ ID=3,  Name="Salt", Price=65 },
                new IngredientEntity{ ID=4,  Name="Pepper", Price=20 },
                new IngredientEntity{ ID=5,  Name="Potato", Price=15 },
                new IngredientEntity{ ID=6,  Name="Pasta", Price=45 },
                new IngredientEntity{ ID=7,  Name="Rice", Price=30 },
                new IngredientEntity{ ID=8,  Name="Pork steak", Price=190 },
                new IngredientEntity{ ID=9,  Name="Chicken wing", Price=160 },
                new IngredientEntity{ ID=10, Name="Salmon", Price=340 },
                new IngredientEntity{ ID=11, Name="Mackerel", Price=260 },
                new IngredientEntity{ ID=12, Name="Parmesan", Price=90 },
                new IngredientEntity{ ID=13, Name="Milk", Price=70 }
            };
            foreach (var item in ingredients)
                context.Ingredient.Add(item);
            context.SaveChanges();

            var menu = new MenuEntity[]
            {
                new MenuEntity{ ID=1,  Name="Chicken Strips", Price=150, IngredientsID="4,5,1,6" },
                new MenuEntity{ ID=2,  Name="French Dip", Price=80, IngredientsID="3,2,6,9" },
                new MenuEntity{ ID=3,  Name="Cobb Salad", Price=50, IngredientsID="13,1,7,10,12,2" },
                new MenuEntity{ ID=4,  Name="Meat Loaf", Price=280, IngredientsID="1,2,3" },
                new MenuEntity{ ID=5,  Name="Cannoli", Price=330, IngredientsID="9,12" },
                new MenuEntity{ ID=6,  Name="ClubHouse", Price=500, IngredientsID="11,12,5,7,9,13" },
                new MenuEntity{ ID=7,  Name="Roast Pork", Price=490, IngredientsID="5,4,6,9,1" },
                new MenuEntity{ ID=8,  Name="Roast Beef", Price=550, IngredientsID="11,3,8,6,4,9" },
                new MenuEntity{ ID=9,  Name="White Pizza", Price=240, IngredientsID="12,13,8,9,5,7,4" },
                new MenuEntity{ ID=10, Name="Hamburger", Price=100, IngredientsID="11,4" }
            };
            foreach (var item in menu)
                context.Menu.Add(item);
            context.SaveChanges();

            var dishes = new DishEntity[]
            {
                new DishEntity{ID=1,  OrderID=2, DishName="Chicken Strips", DateOfOrdering=new DateTime(2023, 5, 15, 16, 59, 00), IsDone=true,  IsTakenAway=true,  IsPrioritized=false},
                new DishEntity{ID=2,  OrderID=4, DishName="French Dip",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=3,  OrderID=8, DishName="Cobb Salad",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=4,  OrderID=1, DishName="Meat Loaf",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00), IsDone=true,  IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=5,  OrderID=6, DishName="Cannoli",        DateOfOrdering=new DateTime(2023, 5, 15, 20, 26, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=6,  OrderID=1, DishName="ClubHouse",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 20, 00), IsDone=true,  IsTakenAway=true,  IsPrioritized=false},
                new DishEntity{ID=7,  OrderID=3, DishName="Roast Pork",     DateOfOrdering=new DateTime(2023, 5, 15, 19, 40, 00), IsDone=false, IsTakenAway=false, IsPrioritized=true },
                new DishEntity{ID=8,  OrderID=3, DishName="Roast Beef",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 23, 00), IsDone=true,  IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=9,  OrderID=7, DishName="White Pizza",    DateOfOrdering=new DateTime(2023, 5, 15, 20, 25, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=10, OrderID=5, DishName="Hamburger",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishEntity{ID=11, OrderID=2, DishName="Meat Loaf",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 40, 00), IsDone=true,   IsTakenAway=true,  IsPrioritized=false},
                new DishEntity{ID=12, OrderID=5, DishName="Chicken Strips", DateOfOrdering=new DateTime(2023, 5, 15, 20, 33, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false},
                new DishEntity{ID=13, OrderID=4, DishName="Hamburger",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 30, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false},
                new DishEntity{ID=14, OrderID=6, DishName="Roast Pork",     DateOfOrdering=new DateTime(2023, 5, 15, 20, 10, 00), IsDone=true,   IsTakenAway=true,  IsPrioritized=false},
                new DishEntity{ID=15, OrderID=8, DishName="ClubHouse",      DateOfOrdering=new DateTime(2023, 5, 15, 20, 19, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false}
            };
            foreach (var item in dishes)
                context.Dish.Add(item);
            context.SaveChanges();

            var order = new OrderEntity[]
            {
                new OrderEntity{ ID=1,  TableID=3,  Open=true,  Message="" },
                new OrderEntity{ ID=2,  TableID=2,  Open=true,  Message="" },
                new OrderEntity{ ID=3,  TableID=1,  Open=true,  Message="" },
                new OrderEntity{ ID=4,  TableID=5,  Open=true,  Message="" },
                new OrderEntity{ ID=5,  TableID=8,  Open=true,  Message="" },
                new OrderEntity{ ID=6,  TableID=7,  Open=true,  Message="" },
                new OrderEntity{ ID=7,  TableID=4,  Open=true,  Message="" },
                new OrderEntity{ ID=8,  TableID=6,  Open=true,  Message="" },
                new OrderEntity{ ID=9,  TableID=1,  Open=true,  Message="abc" }
            };
            foreach (var item in order)
                context.Order.Add(item);
            context.SaveChanges();

            var tables = new TableEntity[]
            {
                new TableEntity{ ID=1, IsOccupied=true,  AmountOfGuests=3, IsPaid=false, OrderCost=2010 },
                new TableEntity{ ID=2, IsOccupied=true,  AmountOfGuests=2, IsPaid=false, OrderCost=500  },
                new TableEntity{ ID=3, IsOccupied=false, AmountOfGuests=0, IsPaid=false, OrderCost=0    },
                new TableEntity{ ID=4, IsOccupied=false, AmountOfGuests=0, IsPaid=false, OrderCost=0    },
                new TableEntity{ ID=5, IsOccupied=true,  AmountOfGuests=5, IsPaid=true,  OrderCost=1900 },
                new TableEntity{ ID=6, IsOccupied=true,  AmountOfGuests=4, IsPaid=false, OrderCost=1500 },
                new TableEntity{ ID=7, IsOccupied=true,  AmountOfGuests=5, IsPaid=false, OrderCost=2600 },
                new TableEntity{ ID=8, IsOccupied=true,  AmountOfGuests=1, IsPaid=false, OrderCost=250  }
            };
            foreach (var item in tables)
                context.Table.Add(item);
            context.SaveChanges();


        }
    }
}


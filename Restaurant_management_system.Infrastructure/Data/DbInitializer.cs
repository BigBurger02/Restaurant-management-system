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

            if (context.Ingredient.Any())
                return;

            var ingredients = new IngredientEntity[]
            {
                new IngredientEntity{ Name="Water", Price=1 },
                new IngredientEntity{ Name="Oil", Price=3 },
                new IngredientEntity{ Name="Salt", Price=2 },
                new IngredientEntity{ Name="Pepper", Price=1 },
                new IngredientEntity{ Name="Potato", Price=5 },
                new IngredientEntity{ Name="Pasta", Price=16 },
                new IngredientEntity{ Name="Rice", Price=8 },
                new IngredientEntity{ Name="Pork steak", Price=40 },
                new IngredientEntity{ Name="Chicken wing", Price=36 },
                new IngredientEntity{ Name="Salmon", Price=54 },
                new IngredientEntity{ Name="Mackerel", Price=23 },
                new IngredientEntity{ Name="Parmesan", Price=13 },
                new IngredientEntity{ Name="Milk", Price=21 },
                new IngredientEntity{ Name="Tomato", Price=4 },
                new IngredientEntity{ Name="Cucumber", Price=3 },
                new IngredientEntity{ Name="Salad", Price=10 },
                new IngredientEntity{ Name="Bacon", Price=68 },
                new IngredientEntity{ Name="Egg", Price=10 },
                new IngredientEntity{ Name="Avocado", Price=9 },
                new IngredientEntity{ Name="Beef", Price=38 },
                new IngredientEntity{ Name="Pork", Price=41 },
                new IngredientEntity{ Name="Flour", Price=14 },
                new IngredientEntity{ Name="Sugar", Price=3 },
                new IngredientEntity{ Name="Creamy filling", Price=24 },
                new IngredientEntity{ Name="Bread", Price=10 },
                new IngredientEntity{ Name="Cheese", Price=10 },
                new IngredientEntity{ Name="Pork cutlet", Price=80 }
            };
            foreach (var item in ingredients)
                context.Ingredient.Add(item);
            context.SaveChanges();

            var menu = new DishInMenuEntity[]
            {
                new DishInMenuEntity{ Name="Chicken Strips", Price=250 },
                new DishInMenuEntity{ Name="French Dip", Price=116 },
                new DishInMenuEntity{ Name="Cobb Salad", Price=99 },
                new DishInMenuEntity{ Name="Meat Loaf", Price=298 },
                new DishInMenuEntity{ Name="Cannoli", Price=100 },
                new DishInMenuEntity{ Name="ClubHouse", Price=199 },
                new DishInMenuEntity{ Name="Roast Pork", Price=270 },
                new DishInMenuEntity{ Name="Roast Beef", Price=320 },
                new DishInMenuEntity{ Name="White Pizza", Price=150 },
                new DishInMenuEntity{ Name="Hamburger", Price=120 }
            };
            foreach (var item in menu)
                context.DishInMenu.Add(item);
            context.SaveChanges();

            var menuIngredients = new IngredientForDishInMenuEntity[]
            {
                new IngredientForDishInMenuEntity{ DishInMenuID=1, IngredientID=2 },
                new IngredientForDishInMenuEntity{ DishInMenuID=1, IngredientID=3 },
                new IngredientForDishInMenuEntity{ DishInMenuID=1, IngredientID=4 },
                new IngredientForDishInMenuEntity{ DishInMenuID=1, IngredientID=9 },
                new IngredientForDishInMenuEntity{ DishInMenuID=2, IngredientID=3 },
                new IngredientForDishInMenuEntity{ DishInMenuID=2, IngredientID=4 },
                new IngredientForDishInMenuEntity{ DishInMenuID=2, IngredientID=5 },
                new IngredientForDishInMenuEntity{ DishInMenuID=2, IngredientID=12 },
                new IngredientForDishInMenuEntity{ DishInMenuID=3, IngredientID=14 },
                new IngredientForDishInMenuEntity{ DishInMenuID=3, IngredientID=15 },
                new IngredientForDishInMenuEntity{ DishInMenuID=3, IngredientID=16 },
                new IngredientForDishInMenuEntity{ DishInMenuID=3, IngredientID=17 },
                new IngredientForDishInMenuEntity{ DishInMenuID=3, IngredientID=18 },
                new IngredientForDishInMenuEntity{ DishInMenuID=3, IngredientID=19 },
                new IngredientForDishInMenuEntity{ DishInMenuID=4, IngredientID=2 },
                new IngredientForDishInMenuEntity{ DishInMenuID=4, IngredientID=3 },
                new IngredientForDishInMenuEntity{ DishInMenuID=4, IngredientID=4 },
                new IngredientForDishInMenuEntity{ DishInMenuID=4, IngredientID=20 },
                new IngredientForDishInMenuEntity{ DishInMenuID=4, IngredientID=21 },
                new IngredientForDishInMenuEntity{ DishInMenuID=4, IngredientID=18 },
                new IngredientForDishInMenuEntity{ DishInMenuID=5, IngredientID=22 },
                new IngredientForDishInMenuEntity{ DishInMenuID=5, IngredientID=23 },
                new IngredientForDishInMenuEntity{ DishInMenuID=5, IngredientID=24 },
                new IngredientForDishInMenuEntity{ DishInMenuID=5, IngredientID=18 },
                new IngredientForDishInMenuEntity{ DishInMenuID=6, IngredientID=25 },
                new IngredientForDishInMenuEntity{ DishInMenuID=6, IngredientID=14 },
                new IngredientForDishInMenuEntity{ DishInMenuID=6, IngredientID=17 },
                new IngredientForDishInMenuEntity{ DishInMenuID=6, IngredientID=26 },
                new IngredientForDishInMenuEntity{ DishInMenuID=6, IngredientID=16 },
                new IngredientForDishInMenuEntity{ DishInMenuID=7, IngredientID=8 },
                new IngredientForDishInMenuEntity{ DishInMenuID=7, IngredientID=5 },
                new IngredientForDishInMenuEntity{ DishInMenuID=7, IngredientID=4 },
                new IngredientForDishInMenuEntity{ DishInMenuID=7, IngredientID=3 },
                new IngredientForDishInMenuEntity{ DishInMenuID=7, IngredientID=2 },
                new IngredientForDishInMenuEntity{ DishInMenuID=8, IngredientID=20 },
                new IngredientForDishInMenuEntity{ DishInMenuID=8, IngredientID=5 },
                new IngredientForDishInMenuEntity{ DishInMenuID=8, IngredientID=4 },
                new IngredientForDishInMenuEntity{ DishInMenuID=8, IngredientID=3 },
                new IngredientForDishInMenuEntity{ DishInMenuID=8, IngredientID=2 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=22 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=23 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=2 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=3 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=4 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=5 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=14 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=12 },
                new IngredientForDishInMenuEntity{ DishInMenuID=9, IngredientID=17 },
                new IngredientForDishInMenuEntity{ DishInMenuID=10, IngredientID=25 },
                new IngredientForDishInMenuEntity{ DishInMenuID=10, IngredientID=27 },
                new IngredientForDishInMenuEntity{ DishInMenuID=10, IngredientID=26 },
                new IngredientForDishInMenuEntity{ DishInMenuID=10, IngredientID=14 },
                new IngredientForDishInMenuEntity{ DishInMenuID=10, IngredientID=16 }
            };
            foreach (var item in menuIngredients)
                context.IngredientForDishInMenu.Add(item);
            context.SaveChanges();

            var dishes = new DishInOrderEntity[]
            {
                new DishInOrderEntity{ OrderID=2, DishID=1, DateOfOrdering=new DateTime(2023, 5, 15, 16, 59, 00), IsDone=true,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ OrderID=4, DishID=2, DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=8, DishID=3, DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=1, DishID=4, DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00), IsDone=true,  IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=6, DishID=5, DateOfOrdering=new DateTime(2023, 5, 15, 20, 26, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=1, DishID=6, DateOfOrdering=new DateTime(2023, 5, 15, 20, 20, 00), IsDone=true,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ OrderID=3, DishID=7, DateOfOrdering=new DateTime(2023, 5, 15, 19, 40, 00), IsDone=false, IsTakenAway=false, IsPrioritized=true },
                new DishInOrderEntity{ OrderID=3, DishID=8, DateOfOrdering=new DateTime(2023, 5, 15, 20, 23, 00), IsDone=true,  IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=7, DishID=9, DateOfOrdering=new DateTime(2023, 5, 15, 20, 25, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=5, DishID=10, DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ OrderID=2, DishID=4, DateOfOrdering=new DateTime(2023, 5, 15, 20, 40, 00), IsDone=true,   IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ OrderID=5, DishID=1, DateOfOrdering=new DateTime(2023, 5, 15, 20, 33, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ OrderID=4, DishID=10, DateOfOrdering=new DateTime(2023, 5, 15, 20, 30, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ OrderID=6, DishID=7, DateOfOrdering=new DateTime(2023, 5, 15, 20, 10, 00), IsDone=true,   IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ OrderID=8, DishID=6, DateOfOrdering=new DateTime(2023, 5, 15, 20, 19, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false}
            };
            foreach (var item in dishes)
                context.DishInOrder.Add(item);
            context.SaveChanges();

            var order = new OrderInTableEntity[]
            {
                new OrderInTableEntity{ TableID=3,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=2,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=1,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=5,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=8,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=7,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=4,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=6,  Open=true,  Message="" },
                new OrderInTableEntity{ TableID=1,  Open=true,  Message="" }
            };
            foreach (var item in order)
                context.OrderInTable.Add(item);
            context.SaveChanges();

            var tables = new TableEntity[]
            {
                new TableEntity{ IsOccupied=true,  AmountOfGuests=3, IsPaid=false, OrderCost=0 },
                new TableEntity{ IsOccupied=true,  AmountOfGuests=2, IsPaid=false, OrderCost=0 },
                new TableEntity{ IsOccupied=false, AmountOfGuests=0, IsPaid=false, OrderCost=0 },
                new TableEntity{ IsOccupied=false, AmountOfGuests=0, IsPaid=false, OrderCost=0 },
                new TableEntity{ IsOccupied=true,  AmountOfGuests=5, IsPaid=true,  OrderCost=0 },
                new TableEntity{ IsOccupied=true,  AmountOfGuests=4, IsPaid=false, OrderCost=0 },
                new TableEntity{ IsOccupied=true,  AmountOfGuests=5, IsPaid=false, OrderCost=0 },
                new TableEntity{ IsOccupied=true,  AmountOfGuests=1, IsPaid=false, OrderCost=0 }
            };
            foreach (var item in tables)
                context.Table.Add(item);
            context.SaveChanges();
        }
    }
}


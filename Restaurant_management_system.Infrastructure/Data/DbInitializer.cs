﻿using System.Net.NetworkInformation;
using Restaurant_management_system.Core.DishesAggregate;
using Restaurant_management_system.Core.TablesAggregate;

namespace Restaurant_management_system.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void Initialize(RestaurantContext context)
        {
            context.Database.EnsureDeleted();// temporary code
            context.Database.EnsureCreated();

            //if (context.DishInOrder.Any())
            //    return;

            var ingredients = new IngredientEntity[]
            {
                new IngredientEntity{ ID=1, Name="Water", Price=1 },
                new IngredientEntity{ ID=2, Name="Oil", Price=3 },
                new IngredientEntity{ ID=3, Name="Salt", Price=2 },
                new IngredientEntity{ ID=4, Name="Pepper", Price=1 },
                new IngredientEntity{ ID=5, Name="Potato", Price=5 },
                new IngredientEntity{ ID=6, Name="Pasta", Price=16 },
                new IngredientEntity{ ID=7, Name="Rice", Price=8 },
                new IngredientEntity{ ID=8, Name="Pork steak", Price=40 },
                new IngredientEntity{ ID=9, Name="Chicken wing", Price=36 },
                new IngredientEntity{ ID=10, Name="Salmon", Price=54 },
                new IngredientEntity{ ID=11, Name="Mackerel", Price=23 },
                new IngredientEntity{ ID=12, Name="Parmesan", Price=13 },
                new IngredientEntity{ ID=13, Name="Milk", Price=21 },
                new IngredientEntity{ ID=14, Name="Tomato", Price=4 },
                new IngredientEntity{ ID=15, Name="Cucumber", Price=3 },
                new IngredientEntity{ ID=16, Name="Salad", Price=10 },
                new IngredientEntity{ ID=17, Name="Bacon", Price=68 },
                new IngredientEntity{ ID=18, Name="Egg", Price=10 },
                new IngredientEntity{ ID=19, Name="Avocado", Price=9 },
                new IngredientEntity{ ID=20, Name="Beef", Price=38 },
                new IngredientEntity{ ID=21, Name="Pork", Price=41 },
                new IngredientEntity{ ID=22, Name="Flour", Price=14 },
                new IngredientEntity{ ID=23, Name="Sugar", Price=3 },
                new IngredientEntity{ ID=24, Name="Creamy filling", Price=24 },
                new IngredientEntity{ ID=25, Name="Bread", Price=10 },
                new IngredientEntity{ ID=26, Name="Cheese", Price=10 },
                new IngredientEntity{ ID=27, Name="Pork cutlet", Price=80 }
            };
            foreach (var item in ingredients)
                context.Ingredient.Add(item);
            context.SaveChanges();

            var menu = new DishInMenuEntity[]
            {
                new DishInMenuEntity{ ID=1, Name="Chicken Strips", Price=250 },
                new DishInMenuEntity{ ID=2, Name="French Dip", Price=116 },
                new DishInMenuEntity{ ID=3, Name="Cobb Salad", Price=99 },
                new DishInMenuEntity{ ID=4, Name="Meat Loaf", Price=298 },
                new DishInMenuEntity{ ID=5, Name="Cannoli", Price=100 },
                new DishInMenuEntity{ ID=6, Name="ClubHouse", Price=199 },
                new DishInMenuEntity{ ID=7, Name="Roast Pork", Price=270 },
                new DishInMenuEntity{ ID=8, Name="Roast Beef", Price=320 },
                new DishInMenuEntity{ ID=9, Name="White Pizza", Price=150 },
                new DishInMenuEntity{ ID=10, Name="Hamburger", Price=120 }
            };
            foreach (var item in menu)
                context.DishInMenu.Add(item);
            context.SaveChanges();

            var menuIngredients = new IngredientForDishInMenuEntity[]
            {
                new IngredientForDishInMenuEntity{ ID=1, DishInMenuID=1, IngredientID=2 },
                new IngredientForDishInMenuEntity{ ID=2, DishInMenuID=1, IngredientID=3 },
                new IngredientForDishInMenuEntity{ ID=3, DishInMenuID=1, IngredientID=4 },
                new IngredientForDishInMenuEntity{ ID=4, DishInMenuID=1, IngredientID=9 },
                new IngredientForDishInMenuEntity{ ID=5, DishInMenuID=2, IngredientID=3 },
                new IngredientForDishInMenuEntity{ ID=6, DishInMenuID=2, IngredientID=4 },
                new IngredientForDishInMenuEntity{ ID=7, DishInMenuID=2, IngredientID=5 },
                new IngredientForDishInMenuEntity{ ID=8, DishInMenuID=2, IngredientID=12 },
                new IngredientForDishInMenuEntity{ ID=9, DishInMenuID=3, IngredientID=14 },
                new IngredientForDishInMenuEntity{ ID=10, DishInMenuID=3, IngredientID=15 },
                new IngredientForDishInMenuEntity{ ID=11, DishInMenuID=3, IngredientID=16 },
                new IngredientForDishInMenuEntity{ ID=12, DishInMenuID=3, IngredientID=17 },
                new IngredientForDishInMenuEntity{ ID=13, DishInMenuID=3, IngredientID=18 },
                new IngredientForDishInMenuEntity{ ID=14, DishInMenuID=3, IngredientID=19 },
                new IngredientForDishInMenuEntity{ ID=15, DishInMenuID=4, IngredientID=2 },
                new IngredientForDishInMenuEntity{ ID=16, DishInMenuID=4, IngredientID=3 },
                new IngredientForDishInMenuEntity{ ID=17, DishInMenuID=4, IngredientID=4 },
                new IngredientForDishInMenuEntity{ ID=18, DishInMenuID=4, IngredientID=20 },
                new IngredientForDishInMenuEntity{ ID=19, DishInMenuID=4, IngredientID=21 },
                new IngredientForDishInMenuEntity{ ID=20, DishInMenuID=4, IngredientID=18 },
                new IngredientForDishInMenuEntity{ ID=21, DishInMenuID=5, IngredientID=22 },
                new IngredientForDishInMenuEntity{ ID=22, DishInMenuID=5, IngredientID=23 },
                new IngredientForDishInMenuEntity{ ID=23, DishInMenuID=5, IngredientID=24 },
                new IngredientForDishInMenuEntity{ ID=24, DishInMenuID=5, IngredientID=18 },
                new IngredientForDishInMenuEntity{ ID=25, DishInMenuID=6, IngredientID=25 },
                new IngredientForDishInMenuEntity{ ID=26, DishInMenuID=6, IngredientID=14 },
                new IngredientForDishInMenuEntity{ ID=27, DishInMenuID=6, IngredientID=17 },
                new IngredientForDishInMenuEntity{ ID=28, DishInMenuID=6, IngredientID=26 },
                new IngredientForDishInMenuEntity{ ID=29, DishInMenuID=6, IngredientID=16 },
                new IngredientForDishInMenuEntity{ ID=30, DishInMenuID=7, IngredientID=8 },
                new IngredientForDishInMenuEntity{ ID=31, DishInMenuID=7, IngredientID=5 },
                new IngredientForDishInMenuEntity{ ID=32, DishInMenuID=7, IngredientID=4 },
                new IngredientForDishInMenuEntity{ ID=33, DishInMenuID=7, IngredientID=3 },
                new IngredientForDishInMenuEntity{ ID=34, DishInMenuID=7, IngredientID=2 },
                new IngredientForDishInMenuEntity{ ID=35, DishInMenuID=8, IngredientID=20 },
                new IngredientForDishInMenuEntity{ ID=36, DishInMenuID=8, IngredientID=5 },
                new IngredientForDishInMenuEntity{ ID=37, DishInMenuID=8, IngredientID=4 },
                new IngredientForDishInMenuEntity{ ID=38, DishInMenuID=8, IngredientID=3 },
                new IngredientForDishInMenuEntity{ ID=39, DishInMenuID=8, IngredientID=2 },
                new IngredientForDishInMenuEntity{ ID=40, DishInMenuID=9, IngredientID=22 },
                new IngredientForDishInMenuEntity{ ID=41, DishInMenuID=9, IngredientID=23 },
                new IngredientForDishInMenuEntity{ ID=42, DishInMenuID=9, IngredientID=2 },
                new IngredientForDishInMenuEntity{ ID=43, DishInMenuID=9, IngredientID=3 },
                new IngredientForDishInMenuEntity{ ID=44, DishInMenuID=9, IngredientID=4 },
                new IngredientForDishInMenuEntity{ ID=45, DishInMenuID=9, IngredientID=5 },
                new IngredientForDishInMenuEntity{ ID=46, DishInMenuID=9, IngredientID=14 },
                new IngredientForDishInMenuEntity{ ID=47, DishInMenuID=9, IngredientID=12 },
                new IngredientForDishInMenuEntity{ ID=48, DishInMenuID=9, IngredientID=17 },
                new IngredientForDishInMenuEntity{ ID=49, DishInMenuID=10, IngredientID=25 },
                new IngredientForDishInMenuEntity{ ID=50, DishInMenuID=10, IngredientID=27 },
                new IngredientForDishInMenuEntity{ ID=51, DishInMenuID=10, IngredientID=26 },
                new IngredientForDishInMenuEntity{ ID=52, DishInMenuID=10, IngredientID=14 },
                new IngredientForDishInMenuEntity{ ID=53, DishInMenuID=10, IngredientID=16 }
            };
            foreach (var item in menuIngredients)
                context.IngredientForDishInMenu.Add(item);
            context.SaveChanges();

            var dishes = new DishInOrderEntity[]
            {
                new DishInOrderEntity{ID=1,  OrderID=2, DishID=1, DateOfOrdering=new DateTime(2023, 5, 15, 16, 59, 00), IsDone=true,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ID=2,  OrderID=4, DishID=2, DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=3,  OrderID=8, DishID=3, DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=4,  OrderID=1, DishID=4, DateOfOrdering=new DateTime(2023, 5, 15, 20, 16, 00), IsDone=true,  IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=5,  OrderID=6, DishID=5, DateOfOrdering=new DateTime(2023, 5, 15, 20, 26, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=6,  OrderID=1, DishID=6, DateOfOrdering=new DateTime(2023, 5, 15, 20, 20, 00), IsDone=true,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ID=7,  OrderID=3, DishID=7, DateOfOrdering=new DateTime(2023, 5, 15, 19, 40, 00), IsDone=false, IsTakenAway=false, IsPrioritized=true },
                new DishInOrderEntity{ID=8,  OrderID=3, DishID=8, DateOfOrdering=new DateTime(2023, 5, 15, 20, 23, 00), IsDone=true,  IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=9,  OrderID=7, DishID=9, DateOfOrdering=new DateTime(2023, 5, 15, 20, 25, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=10, OrderID=5, DishID=10, DateOfOrdering=new DateTime(2023, 5, 15, 20, 03, 00), IsDone=false, IsTakenAway=false, IsPrioritized=false},
                new DishInOrderEntity{ID=11, OrderID=2, DishID=4, DateOfOrdering=new DateTime(2023, 5, 15, 20, 40, 00), IsDone=true,   IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ID=12, OrderID=5, DishID=1, DateOfOrdering=new DateTime(2023, 5, 15, 20, 33, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ID=13, OrderID=4, DishID=10, DateOfOrdering=new DateTime(2023, 5, 15, 20, 30, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ID=14, OrderID=6, DishID=7, DateOfOrdering=new DateTime(2023, 5, 15, 20, 10, 00), IsDone=true,   IsTakenAway=true,  IsPrioritized=false},
                new DishInOrderEntity{ID=15, OrderID=8, DishID=6, DateOfOrdering=new DateTime(2023, 5, 15, 20, 19, 00), IsDone=false,  IsTakenAway=true,  IsPrioritized=false}
            };
            foreach (var item in dishes)
                context.DishInOrder.Add(item);
            context.SaveChanges();

            var order = new OrderInTableEntity[]
            {
                new OrderInTableEntity{ ID=1,  TableID=3,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=2,  TableID=2,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=3,  TableID=1,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=4,  TableID=5,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=5,  TableID=8,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=6,  TableID=7,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=7,  TableID=4,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=8,  TableID=6,  Open=true,  Message="" },
                new OrderInTableEntity{ ID=9,  TableID=1,  Open=true,  Message="" }
            };
            foreach (var item in order)
                context.OrderInTable.Add(item);
            context.SaveChanges();

            var tables = new TableEntity[]
            {
                new TableEntity{ ID=1, IsOccupied=true,  AmountOfGuests=3, IsPaid=false, OrderCost=0 },
                new TableEntity{ ID=2, IsOccupied=true,  AmountOfGuests=2, IsPaid=false, OrderCost=0 },
                new TableEntity{ ID=3, IsOccupied=false, AmountOfGuests=0, IsPaid=false, OrderCost=0 },
                new TableEntity{ ID=4, IsOccupied=false, AmountOfGuests=0, IsPaid=false, OrderCost=0 },
                new TableEntity{ ID=5, IsOccupied=true,  AmountOfGuests=5, IsPaid=true,  OrderCost=0 },
                new TableEntity{ ID=6, IsOccupied=true,  AmountOfGuests=4, IsPaid=false, OrderCost=0 },
                new TableEntity{ ID=7, IsOccupied=true,  AmountOfGuests=5, IsPaid=false, OrderCost=0 },
                new TableEntity{ ID=8, IsOccupied=true,  AmountOfGuests=1, IsPaid=false, OrderCost=0 }
            };
            foreach (var item in tables)
                context.Table.Add(item);
            context.SaveChanges();


        }
    }
}


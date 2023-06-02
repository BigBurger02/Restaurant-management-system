# Restaurant-management-system
This pet project will be a centralized management system of all processes in the restaurant. Users could have access levels (admin, chef, head chef, waiter, analyst, etc.). According to the access level user will have permission to see some information and edit this information. The idea of this system is that all users will interact with the same data but with different representations: the waiter adds order to a specific table, chef sees the only list of dishes he may cook.

## DataBase struct:

- Ingredient table: a list of ingredients. User can pick ingredients for a dish from this table.
    - ID
    - Name
    - Price
- DishInMenu table: the restaurant's menu. User can pick that dishes making the order.
    - ID
    - Name
- IngredientForDishInMenu table: an ingredient for a dish from the menu.
    - ID
    - MenuID
    - IngredientID
- DishInOrder table: dish for the order. Order can have many dishes.
    - ID
    - OrderID
    - DishName
    - DateOfOrdering (DateTime)
    - IsDone (bool)
    - IsTakenAway (bool)
    - IsPrioritized (bool)
- OrderInTable table: order for the table. If Open==false, order will be invisible.
    - ID
    - TableID
    - Open (bool)
    - Message
- Table table: table in the restaurant.
    - ID
    - IsOccupied (bool)
    - AmountOfGuests
    - OrderCost
    - IsPaid (bool)

## Pages list and filling:

- Home
- Tables
    - There may be a list of tables with info:
        - is occupied
        - amount of guests
        - order
        - order cost
        - is paid
    - There may be a list of reservations:
        - date, time
        - amount of guests + amount of tables
        - meeting reason (ordinary, birthday, corporate, wedding, party)
- Dishes
    - There may be a list of dishes the cook needs to cook:
        - time of ordering
        - priority
        - assigned (if the user is the chef, he can see all dishes and assign them to other cooks)
        - ingredients
- Menu
    - There may be a list of all dishes. It will be used for auto calculation of order cost. It also will help the waiter to suggest dishes to the guest
- Supply of products
    - If there is a deficit of any product in the storage, the chef can order it from the supplier
    - The chef can change the normal amount of products in the storage
- Analytics
    - There may be all statistics about guests, their orders, cooking food details (time to cook, cook, etc.)
- Cooks
    - There may be a list of all cooks. The chef can assign a dish to cooks, so he should be able to change names or add another cook
- About

## Roles and page permissions:

- Admin
    - Tables
    - Dishes
    - Menu
    - Supply of products
    - Analytics (readonly)
    - Cooks
- Waiter
    - Tables
    - Menu
    - Dishes (readonly)
- Chef
    - Dishes
    - Menu
    - Supply of products
    - Cooks
- Cook
    - Dishes

## Use cases:

- Serve a guest:
    - Actor: Waiter
    - Flow 0: The waiter sees what table is free and suggests it to the new guest. The waiter takes the order, fills in all info, and takes an order.
    - Flow 1: The guest can change his order, and the waiter can add and delete dishes to the order.
    - Flow 2: When the dish is cooked, the waiter brings it from the kitchen to the guest.
    - Flow 3: If guests have an allergy, the waiter can check the menu to be sure the dish does not have an allergic component.
- Cook a dish:
    - Actor: Chef, cook
    - Flow 1: Cook can see a list of dishes he may cook. He can see the time of ordering and priority. When the dish is done, he can point to it as Cooked.
- Manage dish list:
    - Actor: Chef
    - Flow 1: The chef can assign a dish to any cook or himself.
- Edit menu:
    - Actor: Chef
    - Flow 1: The chef can edit any dish, add or delete.

# Restaurant management system
This is an application for a restaurant or a chain of restaurants (at the moment, the functionality allows you to use it only within one restaurant). It is designed to simplify the work of both the client and the restaurant staff. Employees can create orders and change their status. Visitors can also place an order by adding dishes to the cart.

## Application struct
The application consists of 3 parts:
### Duende Identity Server (Proj: IdentityServer)
The Identity Server takes care of all the user and client authentication work by issuing an AccessToken and an IdentityToken.
The Client has a ClientID and a ClientSecret that it must provide to the IdentityServer and receive an AccessToken. With this token, he can access API endpoints.
Users can also use Identity Server for authentication. The Identity Server has information about the user roles and, based on it, Client allows or does not allow the User to functionality.
### API (Proj: Restaurant_management_system.WebUI)
Initially, all the functionality of the WebUI Project was using controllers, but later I decided to use only the API. Now the API only provides endpoints for visitors that the Client project uses. Later I will transfer everything to the API, but for now, the functionality for restaurant staff is available in the API project without any authentication. The project has been wroten using Clean Architecture.
### Client (Proj: WebClient)
The project through which interaction with the system should take place (for visitors and staff).

## Azure
The projects Identity Server and Restaurant management system are deployed to Azure. The Client project running on localhost can access them.
The application uses two databases. One for the Identity Server, which stores all user data. And one for storing dishes, orders, etc.
Both databases have been deployed on Azure.

## Implemented technologies and features:
- Clean architecture (Core, Infrastructure, WebUI) for WebUI project
- BD: Entity Configuration, DTOs, DataBase Normalization, Azure
- Bootstrap layout
- Auth
    - Deunde IdentityServer
    - OIDC, OAuth
    - Use access_token to grant access to API
    - SSO: Google, Microsoft, Github, Twitter
    - Role-based authorization
- Azure
    - AzureSqlServer
    - KeyVault
    - Logs:
        - Realtime Console Logger
        - File logger
        - Blob logger
        - Application Insights logger
    - CI/CD deploy in GitHub Actions
- WebAPI: swagger, token authentication
- Email sender (Brevo)
- Validation
- Localization (Ukrainian, English, Deutsch)
- CORS Policy
- Logger
- Unit Tests: AutoFixture, Moq, Bogus, FluentAssertions
- Custom cookies

## Feature plan:
- Hangfire
- Polly
- Cache
- SignalR
- Pay system
- Ajax requests
- Pagination, sorting (for menu and ingredients list)
- The ability to use in a chain of restaurants

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
- Manage dish list (not implemented):
    - Actor: Chef
    - Flow 1: The chef can assign a dish to any cook or himself.
- Edit menu:
    - Actor: Chef
    - Flow 1: The chef can edit any dish, add or delete.
- Make an order:
    - Actor: Customer
    - Flow 1: The customer choise dishes from menu, adding it to the cart. He can pay for the order from the application.
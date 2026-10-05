# Pizza Store API

A RESTful backend API for managing pizzas and toppings, built with ASP.NET Core 8 Web API and Entity Framework Core. This project uses an In-Memory database for easy testing without external SQL dependencies.

## Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## How to Build and Run
1. Clone the repository:
   ```bash
   `git clone <YOUR_GITHUB_REPOSITORY_URL>`
2. Navigate to the project directory `cd PizzaStoreApi`
3. Run `dotnet restore` to install dependencies.
4. Run `dotnet run` to start the API.

## How to Test
1. Once the application is running, check the terminal output for the local port (e.g., Now listening on: http://localhost:5xxx).
2. Open a web browser and navigate to the Swagger UI: http://localhost:<port>/swagger
3. Use the interactive Swagger interface to test all GET, POST, PUT, and DELETE endpoints for both Pizzas and Toppings. The endpoints handle duplicate validation and mapping automatically.


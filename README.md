# 🛒 Store.FinalProject A full-featured **E-Commerce REST API** built with **ASP.NET Core**, following **Clean Architecture** principles. The API supports product browsing, basket management, order processing, and user authentication with JWT tokens. --- ## 📋 Table of Contents - [Project Overview](#-project-overview) - [Architecture](#-architecture) - [Solution Structure](#-solution-structure) - [Technologies Used](#-technologies-used) - [Getting Started](#-getting-started) - [API Endpoints](#-api-endpoints) - [Data Models & Schemas](#-data-models--schemas) - [Design Patterns Used](#-design-patterns-used) - [Database & Migrations](#-database--migrations) - [Seeding Data](#-seeding-data) --- ## 📌 Project Overview **Store.FinalProject** is a backend API for an online store. It exposes a comprehensive set of RESTful endpoints for managing: - 🔐 **Authentication** — Register, login, JWT token issuance - 📦 **Products** — Browse, filter, paginate, search products with brand/type support - 🧺 **Basket** — Redis-backed customer basket (add, update, delete) - 📬 **Orders** — Place orders with delivery method and shipping address - 🔧 **Utilities** — Error simulation (Buggy controller), global exception handling The project is structured using **Clean Architecture** with clearly separated layers for core domain logic, data persistence, services, and the web API. --- ## 🏗️ Architecture
┌─────────────────────────────────────────────┐
│             Store.FinalProject              │
│              (ASP.NET Core API)             │
│  Controllers │ Middlewares │ Seeding │ DI   │
└──────────────────┬──────────────────────────┘
                   │
        ┌──────────▼──────────┐
        │    Store.Service     │
        │  (Business Logic)    │
        │  OrderService        │
        │  ProductService      │
        │  TokenService        │
        │  UserService         │
        └──────────┬──────────┘
                   │
        ┌──────────▼──────────┐
        │    Store.Core        │
        │  (Domain / Contracts)│
        │  Entities            │
        │  Interfaces          │
        │  DTOs                │
        │  Specifications      │
        └──────────┬──────────┘
                   │
        ┌──────────▼──────────┐
        │  Store.Repository    │
        │  (Data Access Layer) │
        │  EF Core DbContext   │
        │  Generic Repository  │
        │  Unit of Work        │
        │  Identity            │
        └─────────────────────┘
The solution follows the **Dependency Inversion Principle** — upper layers depend on abstractions (interfaces defined in Store.Core), not concrete implementations. --- ## 📁 Solution Structure The solution contains **4 projects**: ### 1. Store.Core — Domain Layer The heart of the application. Contains all domain entities, interfaces, DTOs, and specifications. Has no dependencies on other projects.
Store.Core/
├── Configurations/
│   ├── DeliveryMethodConfiguration.cs
│   ├── OrderConfiguration.cs
│   └── OrderItemConfiguration.cs
├── Context/
│   └── StoreDbContext.cs
├── Dtos/
│   ├── Auth/
│   │   ├── AddressDto.cs
│   │   ├── LoginDto.cs
│   │   └── RegisterDto.cs
│   ├── Basket/
│   │   └── CustomerBasketDto.cs
│   ├── Orders/
│   │   ├── AddressDto.cs
│   │   ├── OrderDto.cs
│   │   ├── OrderItemDto.cs
│   │   └── OrderToReturnDto.cs
│   ├── ProductBrandDto.cs
│   ├── ProductDto.cs
│   └── ProductTypeDto.cs
├── Entities/
│   ├── Identity/
│   │   ├── Address.cs
│   │   └── AppUser.cs
│   ├── Orders/
│   │   ├── Address.cs
│   │   ├── DeliveryMethod.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── OrderStatus.cs
│   │   └── ProductItemOrder.cs
│   ├── BaseEntity.cs
│   ├── BasketItem.cs
│   ├── CustomerBasket.cs
│   ├── Product.cs
│   ├── ProductBrand.cs
│   └── ProductType.cs
├── Helper/
│   └── PaginationResponse.cs
├── Mapping/
│   ├── Auth/
│   │   └── AuthProfile.cs
│   ├── Basket/
│   │   └── BasketProfile.cs
│   ├── Orders/
│   │   └── OrderProfile.cs
│   └── Products/
│       └── ProductProfile.cs
├── Migrations/
│   ├── 20260212224849_AddProductsTable.cs
│   ├── 20260314202751_AddOrderModuleTables.cs
│   └── StoreDbContextModelSnapshot.cs
├── Repositories.Contract/
│   ├── IBasketRepository.cs
│   ├── IGenericRepository.cs
│   └── IUnitOfWork.cs
├── Services.Contract/
│   ├── IOrderService.cs
│   ├── IProductService.cs
│   ├── ITokenService.cs
│   └── IUserService.cs
└── Specification/
    ├── ISpecification.cs
    └── ProductSpecParams.cs
--- ### 2. Store.Repository — Data Access Layer Handles all database interactions using **Entity Framework Core** and **Redis** for basket storage.
Store.Repository/
├── Basket/
│   └── BasketRepository.cs
├── Identity/
│   ├── Configurations/
│   ├── Context/
│   │   └── StoreIdentityDbContext.cs
│   ├── DataSeed/
│   ├── Migrations/
│   │   ├── 20260221232109_AddIdentityTables.cs
│   │   └── StoreIdentityDbContextModelSnapshot.cs
│   └── StoreIdentityContextSeed.cs
├── Repositories/
│   └── GenericRepository.cs
├── SeedData/
│   ├── brands.json
│   ├── delivery.json
│   ├── products.json
│   └── Types.json
├── Seeding/
│   └── StoreContextSeed.cs
├── Specification/
│   ├── Orders/
│   ├── BaseSpecification.cs
│   ├── ProductSpecification.cs
│   ├── ProductWithCountSpecification.cs
│   └── SpecificationEvaluator.cs
└── UnirOfWork/
    └── UnitOfWork.cs
--- ### 3. Store.Service — Business Logic Layer Contains all service implementations that orchestrate domain logic.
Store.Service/
├── Orders/
│   └── OrderService.cs
└── Services/
    ├── Token/
    │   └── TokenService.cs
    ├── Users/
    └── ProductService.cs
--- ### 4. Store.FinalProject — Presentation Layer (ASP.NET Core Web API) The entry point of the application. Exposes HTTP endpoints and wires up all dependencies.
Store.FinalProject/
├── Controllers/
│   ├── AccountController.cs
│   ├── BasketController.cs
│   ├── BuggyController.cs
│   ├── ErrorController.cs
│   ├── OrderController.cs
│   ├── ProductController.cs
│   └── WeatherForecastController.cs
├── Errors/
│   ├── ApiErrorResponse.cs
│   └── ApiExceptionResponse.cs
├── Extensions/
│   └── UserManagerExtensions.cs
├── MiddleWares/
│   └── ExceptionMiddleware.cs
├── Seeding/
│   └── ApllySeeding.cs
├── appsettings.json
├── Program.cs
└── WeatherForecast.cs
--- ## 🛠️ Technologies Used | Category | Technology | |---|---| | Framework | ASP.NET Core (.NET 8+) | | ORM | Entity Framework Core | | Database | SQL Server (Products & Orders) | | Identity DB | SQL Server (ASP.NET Identity) | | Basket Storage | Redis | | Authentication | JWT Bearer Tokens | | Object Mapping | AutoMapper | | API Documentation | Swagger / OpenAPI 3.0 | | Architecture | Clean Architecture | | Patterns | Repository, Unit of Work, Specification, CQRS-lite | --- ## 🚀 Getting Started ### Prerequisites - [.NET 8 SDK](https://dotnet.microsoft.com/) - SQL Server (local or remote) - Redis server - Visual Studio 2022 or VS Code ### Configuration Update appsettings.json with your connection strings:
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=StoreDb;Trusted_Connection=True;",
    "IdentityConnection": "Server=.;Database=StoreIdentityDb;Trusted_Connection=True;",
    "Redis": "localhost"
  },
  "Token": {
    "Key": "your-secret-key-here",
    "Issuer": "https://localhost:7045"
  }
}
### Running the Project
bash
# Restore dependencies
dotnet restore

# Apply EF Core migrations
dotnet ef database update --project Store.Repository --startup-project Store.FinalProject

# Run the API
dotnet run --project Store.FinalProject
### Swagger UI Once running, navigate to:
https://localhost:7045/swagger
--- ## 📡 API Endpoints ### 🔐 Account | Method | Endpoint | Description | Auth Required | |---|---|---|---| | POST | /api/Account/Login | Login with email & password, returns JWT | ❌ | | POST | /api/Account/Register | Register a new user | ❌ | | GET | /api/Account/GetCurrentUser | Get the currently authenticated user | ✅ | | GET | /api/Account/Address | Get the current user's saved address | ✅ | **Login Request Body:**
json
{
  "email": "user@example.com",
  "password": "P@ssword123"
}
**Register Request Body:**
json
{
  "displayName": "John Doe",
  "email": "user@example.com",
  "password": "P@ssword123"
}
**User Response:**
json
{
  "email": "user@example.com",
  "displayName": "John Doe",
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
--- ### 🧺 Basket | Method | Endpoint | Description | Auth Required | |---|---|---|---| | GET | /api/Basket | Get basket by ID (query param: id) | ❌ | | POST | /api/Basket | Create or update a basket | ❌ | | DELETE | /api/Basket | Delete a basket by ID | ❌ | **GET /api/Basket** — Query Params: | Param | Type | Description | |---|---|---| | id | string | The basket identifier (e.g. basket_001) | **Basket Object (GET Response & POST Body):**
json
{
  "id": "basket_001",
  "items": [
    {
      "id": 1,
      "productName": "Samsung Galaxy S24",
      "quantity": 2,
      "price": 999.99,
      "brand": "Samsung",
      "type": "Phones",
      "pictureUrl": "https://..."
    }
  ],
  "deliveryMethod": 1,
  "paymentIntentId": "pi_xxx",
  "clientSecret": "secret_xxx"
}
--- ### 📬 Order | Method | Endpoint | Description | Auth Required | |---|---|---|---| | POST | /api/Order | Place a new order | ✅ | | GET | /api/Order | Get all orders for current user | ✅ | | GET | /api/Order/{orderId} | Get a specific order by ID | ✅ | | GET | /api/Order/DeliveryMethod | Get all available delivery methods | ✅ | **POST /api/Order — Request Body:**
json
{
  "basketId": "basket_001",
  "deliveryMethodId": 1,
  "shipToAddress": {
    "firstName": "John",
    "lastName": "Doe",
    "street": "123 Main Street",
    "city": "Cairo"
  }
}
--- ### 📦 Product | Method | Endpoint | Description | Auth Required | |---|---|---|---| | GET | /api/Product | Get all products (with filters & pagination) | ❌ | | GET | /api/Product/Brands | Get all product brands | ❌ | | GET | /api/Product/Types | Get all product types | ❌ | | GET | /api/Product/{id} | Get a single product by ID | ❌ | **GET /api/Product — Query Parameters:** | Param | Type | Description | |---|---|---| | Sort | string | Sort order (e.g. priceAsc, priceDesc, name) | | BrandId | integer | Filter by brand ID | | TypeId | integer | Filter by type ID | | pageSize | integer | Number of items per page | | pageIndex | integer | Page number (1-based) | | search | string | Search term for product name | **Product Response:**
json
{
  "id": 1,
  "name": "Angular Speedster Board 2000",
  "description": "A great board for angular developers",
  "pictureUrl": "https://...",
  "price": 200.00,
  "typeName": "Boards",
  "typeId": 1,
  "brandName": "Angular",
  "brandId": 1
}
--- ### 🐛 Buggy (Error Testing) | Method | Endpoint | Description | |---|---|---| | GET | /api/Buggy/NotFound | Returns 404 Not Found | | GET | /api/Buggy/badrequest | Returns 400 Bad Request | | GET | /api/Buggy/badrequest/{id} | Returns validation error | | GET | /api/Buggy/unauthorized | Returns 401 Unauthorized | --- ## 📐 Data Models & Schemas ### AddressDto
json
{
  "firstName": "string",
  "lastName": "string",
  "street": "string",
  "city": "string"
}
### ProductDto
json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "pictureUrl": "string",
  "price": 0,
  "typeName": "string",
  "typeId": 0,
  "brandName": "string",
  "brandId": 0
}
### CustomerBasketDto
json
{
  "id": "string",
  "items": [
    {
      "id": 0,
      "productName": "string",
      "quantity": 0,
      "price": 0,
      "brand": "string",
      "type": "string",
      "pictureUrl": "string"
    }
  ],
  "deliveryMethod": 0,
  "paymentIntentId": "string",
  "clientSecret": "string"
}
### OrderDto
json
{
  "basketId": "string",
  "deliveryMethodId": 0,
  "shipToAddress": {
    "firstName": "string",
    "lastName": "string",
    "street": "string",
    "city": "string"
  }
}
### UserDto / Login Response
json
{
  "email": "string",
  "displayName": "string",
  "token": "string"
}
### RegisterDto
json
{
  "displayName": "string",
  "email": "string",
  "password": "string"
}
### ApiErrorResponse
json
{
  "statusCode": 0,
  "message": "string"
}
--- ## 🧩 Design Patterns Used ### 1. Repository Pattern Abstracts data access logic behind interfaces (IGenericRepository<T>, IBasketRepository). Controllers and services never talk directly to DbContext. ### 2. Unit of Work Pattern IUnitOfWork / UnitOfWork.cs coordinates multiple repository operations in a single transaction, ensuring data consistency across related tables. ### 3. Specification Pattern Instead of polluting repositories with query logic, specifications encapsulate query criteria: - ProductSpecification.cs — Filter, sort, include Brand and Type - ProductWithCountSpecification.cs — For pagination count - BaseSpecification.cs — Base implementation with criteria, includes, ordering, and paging - SpecificationEvaluator.cs — Applies the specification to an IQueryable ### 4. AutoMapper Profiles Mapping between domain entities and DTOs is handled by dedicated profiles: - AuthProfile.cs — AppUser ↔ UserDto - BasketProfile.cs — CustomerBasket ↔ CustomerBasketDto - OrderProfile.cs — Order ↔ OrderToReturnDto - ProductProfile.cs — Product ↔ ProductDto ### 5. Global Exception Handling A custom ExceptionMiddleware catches all unhandled exceptions and returns consistent ApiExceptionResponse JSON objects — no raw stack traces leak to the client. --- ## 🗄️ Database & Migrations The project uses **two separate databases**: ### Store Database (Products & Orders) Managed by StoreDbContext. | Migration | Description | |---|---| | 20260212224849_AddProductsTable | Creates Products, Brands, Types tables | | 20260314202751_AddOrderModuleTables | Creates Orders, OrderItems, DeliveryMethods tables | ### Identity Database (Users & Auth) Managed by StoreIdentityDbContext. | Migration | Description | |---|---| | 20260221232109_AddIdentityTables | Creates ASP.NET Identity tables (Users, Roles, etc.) | --- ## 🌱 Seeding Data On application startup, ApllySeeding.cs is called from Program.cs to seed initial data from JSON files in Store.Repository/SeedData/: | File | Seeds | |---|---| | brands.json | Product brands (e.g. Angular, NetCore, VS Code) | | Types.json | Product types (e.g. Boards, Hats, Boots) | | products.json | Sample product catalog | | delivery.json | Delivery methods (e.g. UPS Ground, UPS Second Day Air) | Identity seeding is handled by StoreIdentityContextSeed.cs and StoreContextSeed.cs. --- ## 📝 Notes - The BuggyController is intended for **development/testing only** and should be removed or secured before production deployment. - The WeatherForecastController is the default ASP.NET template scaffold and can be safely removed. - JWT tokens are issued on login/register and must be passed as Authorization: Bearer <token> headers for protected endpoints. - The basket uses **Redis** for storage, meaning baskets are ephemeral — they will be cleared if the Redis instance restarts without persistence configured. ---

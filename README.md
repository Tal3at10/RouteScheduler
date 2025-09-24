# RouteScheduler

Robust, production-ready route scheduling and management system built with .NET 8.  
It provides RESTful APIs for managing drivers, routes, and schedules with availability checks, pagination, and clean architecture patterns (Dependency Injection, AutoMapper, Unit of Work, and Generic Repository).  
The solution supports both MongoDB and relational SQL databases (SQLite/Postgres) via a pluggable persistence layer.

---

## Contents
- [Project Overview](#project-overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

---

## Project Overview

RouteScheduler is an API-first system for planning and managing transportation routes, drivers, and schedules.

- Manage Drivers, Routes, and Schedules with clean, versioned endpoints
- Scheduling logic with driver availability checks and conflict prevention
- Pagination for list endpoints for efficient data retrieval
- Pluggable persistence: use MongoDB or a SQL database (SQLite/Postgres)
- Clean layering with DTOs and AutoMapper mapping profile
- Dependency Injection for services and repositories
- Unit of Work and Generic Repository patterns for consistent data access

### Key Features
- Route management endpoints (CRUD, search, pagination)
- Driver management endpoints (CRUD, history, availability)
- Schedule management endpoints (CRUD, filters, availability checks)
- Pagination support across list endpoints
- AutoMapper-based mapping profile
- Centralized exception handling middleware (GlobalExceptionMiddleware)

---

## Tech Stack

- **.NET**: .NET 8 (C#)
- **Databases**: MongoDB and SQL (SQLite/Postgres)
- **ORM**: Entity Framework Core (for SQL option)
- **Object Mapping**: AutoMapper
- **Dependency Management**: Built-in Dependency Injection
- **Patterns**: Unit of Work + Generic Repository
- **API**: RESTful API using Controllers + Swagger/OpenAPI
- **Pagination**: Custom `PaginationParams` and `PagedResult<T>` implementation

---

## Project Structure

The solution is organized into clear layers for maintainability and testability:

- `RouteScheduler.APIs`
  - Startup (`Program.cs`), middleware (`GlobalExceptionMiddleware`), extensions, and Swagger configuration
  - Hosts the REST controllers

- `RouteScheduler.APIs.Controllers`
  - Controllers for the public API
    - `DriverController`, `RouteController`, `ScheduleController`

- `RouteScheduler.Core.Application`
  - Application layer services and DI registration
  - Services: `DriverService`, `RouteService`, `ScheduleService`
  - Mapping: `Mapping/MappingProfile.cs` (AutoMapper)
  - `DependencyInjection.cs` for wiring application services

- `RouteScheduler.Core.Application.Abstraction`
  - Service interfaces (`IDriverService`, `IRouteService`, `IScheduleService`, `IServiceManager`)
  - Common models: `PaginationParams`, `PagedResult<T>`
  - DTOs for Drivers, Routes, and Schedules

- `RouteScheduler.Core.Domain`
  - Domain entities: `Driver`, `Route`, `Schedule`
  - Contracts: `IGenericRepository<T>`, `IUnitOfWork`, `IContextInitializer`
  - Common base entity: `BaseAuditableEntity`

- `RouteScheduler.Infrastructure.Persistence`
  - EF Core DbContext and configurations (SQL path)
  - Mongo persistence (MongoContext, settings, repositories)
  - Repositories: `GenericRepository<T>` and Mongo equivalent
  - Units of Work: `UnitOfWork` (SQL) and `MongoUnitOfWork` (Mongo)
  - `DependencyInjection.cs` to register the chosen persistence

---

## Getting Started

### Prerequisites
- .NET SDK 8.0+
- One database option:
  - MongoDB 6+ (local or remote)
  - OR a SQL database (SQLite or Postgres)
- Optional (for EF Core migrations):
  - `dotnet-ef` CLI: `dotnet tool install --global dotnet-ef`

### Installation
```bash
git clone <your-fork-or-repo-url>
cd RouteScheduler
dotnet restore
````

### Configure Database

Set the connection settings in `RouteScheduler.APIs/appsettings.json`.

* MongoDB example:

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RouteSchedulerDb"
  }
}
```

* SQL (SQLite/Postgres) example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=route_scheduler;Username=postgres;Password=postgres"
  }
}
```

Select the desired persistence in `RouteScheduler.Infrastructure.Persistence/DependencyInjection.cs` configuration and ensure `AddPersistenceServices` uses the correct backing store (SQL or Mongo).

### Run Migrations (SQL option)

```bash
# From the solution root (ensure DefaultConnection is set)
dotnet ef database update --project RouteScheduler.Infrastructure.Persistence --startup-project RouteScheduler.APIs
```

### Run the API locally

```bash
cd RouteScheduler.APIs
dotnet run
```

Swagger UI will be available at `http://localhost:5000/swagger` (or the port shown in console).

---

## Usage

The API is grouped into Drivers, Routes, and Schedules.
All list endpoints support pagination using `pageNumber` and `pageSize`.
Example: `GET /api/driver/paged?pageNumber=1&pageSize=10`.

### Driver Management

| Method | Endpoint                                        | Description                 |
| ------ | ----------------------------------------------- | --------------------------- |
| GET    | `/api/driver`                                   | Get all drivers             |
| GET    | `/api/driver/paged?pageNumber=&pageSize=`       | Get drivers with pagination |
| GET    | `/api/driver/{id}`                              | Get driver by ID            |
| POST   | `/api/driver`                                   | Create a driver             |
| PUT    | `/api/driver/{id}`                              | Update a driver             |
| DELETE | `/api/driver/{id}`                              | Delete a driver             |
| GET    | `/api/driver/{id}/history`                      | Get driver route history    |
| GET    | `/api/driver/{id}/availability?date=YYYY-MM-DD` | Check driver availability   |

Example: Create Driver

```http
POST /api/driver
Content-Type: application/json

{
  "name": "Jane Doe",
  "licenseNumber": "DR-12345",
  "phone": "+1-555-0101"
}
```

---

### Routes Management

| Method | Endpoint                                                      | Description                                     |
| ------ | ------------------------------------------------------------- | ----------------------------------------------- |
| GET    | `/api/route`                                                  | Get all routes                                  |
| GET    | `/api/route/paged?pageNumber=&pageSize=&origin=&destination=` | Get routes with pagination and optional filters |
| GET    | `/api/route/{id}`                                             | Get route by ID                                 |
| POST   | `/api/route`                                                  | Create a route                                  |
| PUT    | `/api/route/{id}`                                             | Update a route                                  |
| DELETE | `/api/route/{id}`                                             | Delete a route                                  |
| GET    | `/api/route/search?origin=&destination=`                      | Search routes by origin/destination             |

Example: Create Route

```http
POST /api/route
Content-Type: application/json

{
  "origin": "Berlin",
  "destination": "Munich",
  "distanceKm": 585
}
```

---

### Schedules

| Method | Endpoint                                                | Description                                    |
| ------ | ------------------------------------------------------- | ---------------------------------------------- |
| GET    | `/api/schedule`                                         | Get all schedules                              |
| GET    | `/api/schedule/{id}`                                    | Get schedule by ID                             |
| POST   | `/api/schedule`                                         | Create a schedule (checks driver availability) |
| PUT    | `/api/schedule/{id}`                                    | Update a schedule                              |
| DELETE | `/api/schedule/{id}`                                    | Delete a schedule                              |
| GET    | `/api/schedule/driver/{driverId}`                       | Get schedules by driver                        |
| GET    | `/api/schedule/date/{date}`                             | Get schedules by date                          |
| GET    | `/api/schedule/status/{status}`                         | Get schedules by status                        |
| GET    | `/api/schedule/availability/{driverId}?date=YYYY-MM-DD` | Check driver availability by date              |

---

### Pagination Model

```json
{
  "items": [ /* DTO[] */ ],
  "totalCount": 123,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

## Contributing

Contributions are welcome!
To propose changes:

* Fork the repository and create a feature branch
* Follow existing code style and architecture patterns
* Add or update documentation as needed
* Open a pull request describing your changes

---




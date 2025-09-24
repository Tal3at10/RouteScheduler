## RouteScheduler APIs

### Introduction
RouteScheduler is a route scheduling and driver management system. It provides APIs to manage drivers, routes, and schedules, including availability checks and optional features like pagination and driver history.

### Features
- **Driver management**: Create, read, update, delete drivers; check availability; history of driven routes; optional pagination.
- **Route management**: CRUD routes; search by origin/destination; optional pagination.
- **Scheduling**: Create schedules with conflict/availability checks; query by driver, date, and status.
- **Database integration**: Pluggable persistence via configuration: SqlServer, Sqlite, Postgres, or MongoDb.
- **API documentation**: Swagger/OpenAPI with UI.

### Technology Stack
- **.NET 8** / **ASP.NET Core**
- **Entity Framework Core** (SqlServer/Sqlite/Postgres)
- **MongoDB Driver** (MongoDb option)
- **AutoMapper**
- **Swagger / OpenAPI**

### Architecture
Clean, layered architecture:
- **API layer** (`RouteScheduler.APIs`, `RouteScheduler.APIs.Controllers`): Controllers, middleware, Swagger.
- **Application layer** (`RouteScheduler.Core.Application`): Services, mappings.
- **Domain layer** (`RouteScheduler.Core.Domain`): Entities and contracts (`IGenericRepository`, `IUnitOfWork`).
- **Infrastructure/Persistence** (`RouteScheduler.Infrastructure.Persistence`): EF Core context, repositories, UnitOfWork; Mongo settings/context and repositories; provider selection via DI.

### Setup and Running the Project
1. Prerequisites:
   - .NET SDK 8+
   - For MongoDb option: MongoDB instance/Atlas URI
   - For Postgres/SqlServer/Sqlite: respective DB engines or connection strings
2. Configure `RouteScheduler.APIs/appsettings.json`:
   - Set `DatabaseProvider` to one of: `SqlServer`, `Sqlite`, `Postgres`, `MongoDb`.
   - Set the related connection string in `ConnectionStrings` (e.g., `SqlServer`, `Sqlite`, `Postgres`, `MongoDb`).
3. Build and run:
```bash
dotnet build RouteScheduler.sln
dotnet run --project RouteScheduler.APIs
```
4. Open Swagger UI:
   - HTTPS: `https://localhost:7089/`
   - HTTP: `http://localhost:5280/`

### API Endpoints (summary)

Drivers (`/api/driver`)
- `GET /` — list all drivers
- `GET /paged?pageNumber=&pageSize=` — paged drivers
- `GET /{id}` — get a driver
- `POST /` — create driver
- `PUT /{id}` — update driver
- `DELETE /{id}` — delete driver
- `GET /{id}/history` — routes driven by driver
- `GET /{id}/availability?date=` — availability check

Routes (`/api/route`)
- `GET /` — list all routes
- `GET /paged?pageNumber=&pageSize=&origin=&destination=` — paged routes with filters
- `GET /{id}` — get a route
- `POST /` — create route
- `PUT /{id}` — update route
- `DELETE /{id}` — delete route
- `GET /search?origin=&destination=` — search routes

Schedules (`/api/schedule`)
- `GET /` — list all schedules
- `GET /{id}` — get a schedule
- `POST /` — create schedule (validates driver availability)
- `PUT /{id}` — update schedule
- `DELETE /{id}` — delete schedule
- `GET /driver/{driverId}` — schedules for driver
- `GET /date/{date}` — schedules for date
- `GET /status/{status}` — schedules by status
- `GET /availability/{driverId}?date=` — availability check

### Contact
Name: Mahmoud Talaat Mahmoud
Email: mahmoud.talaat.dev@gmail.com
LinkedIn: linkedin.com/in/mahmoud-talaat-9a2487295/

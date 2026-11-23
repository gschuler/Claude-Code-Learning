# CLAUDE.md

Guidance for Claude when working in this repository.

## What this is
A learning repo with two independent ASP.NET Core (.NET 10, `net10.0`) sample APIs in the buy/checkout space:
- `Marketplace/` – sellers and products catalog.
- `Checkout/` – cart → order → pay/cancel.

Each folder is its own solution (`<Name>.slnx`). They do not reference each other yet.

## Architecture rules (Clean Architecture)
- **Domain** (`src/<Name>.Domain`): entities, value objects, `DomainException`. No project or package references. Business rules live here, inside the entities (private setters, factory methods like `Product.Create`, behavior methods like `Cart.Checkout()`).
- **Application** (`src/<Name>.Application`): one service class per aggregate (`ProductService`, `CartService`…), request/response records next to the service, repository + `IUnitOfWork` interfaces in `Abstractions/`, `NotFoundException` in `Common/`. References Domain only (plus `Microsoft.Extensions.DependencyInjection.Abstractions` for `AddApplication()`).
- **Infrastructure** (`src/<Name>.Infrastructure`): EF Core with SQLite. `DbContext` implements `IUnitOfWork`; mappings are `IEntityTypeConfiguration<T>` classes in `Persistence/Configurations.cs`; repositories are `internal` in `Persistence/Repositories.cs`. Exposes `AddInfrastructure()` and `EnsureDatabaseCreated()`.
- **Api** (`src/<Name>.Api`): Minimal API endpoint groups in `Endpoints/` (one static `Map<X>Endpoints` class per resource), `Errors/ApiExceptionHandler.cs` maps `DomainException → 400` and `NotFoundException → 404` as ProblemDetails. Endpoints stay thin: call a service, return a result. In Development, OpenAPI is served at `/openapi/v1.json` and the Scalar UI (`Scalar.AspNetCore`) at `/scalar`.
- Dependencies only point inward. Never reference Infrastructure or EF Core from Domain/Application.

## Database
- SQLite, connection string `ConnectionStrings:Default` in `appsettings.json` (Docker overrides it with `ConnectionStrings__Default`).
- Schema is created with `Database.EnsureCreated()` at startup — there are **no migrations yet**. If the model changes, delete the local `.db` file. Moving to EF migrations is a planned next step.
- Sample data: `Persistence/SeedData.cs`, applied with `HasData` in the entity configurations (anonymous objects, since entities have private setters; owned types are seeded via their owner's FK, e.g. `ProductId`). Ids are fixed and referenced by the `.http` files; Checkout's seed reuses Marketplace's product ids. Integration test databases are seeded too, so tests must not assume empty tables.
- The DbContext reads the connection string lazily from `IConfiguration` so integration tests can override it.

## Tests
- `tests/<Name>.UnitTests`: xUnit tests for Domain rules and Application services, using hand-written fakes in `Application/Fakes.cs` (no mocking library).
- `tests/<Name>.IntegrationTests`: `WebApplicationFactory<Program>` against a shared-cache in-memory SQLite DB kept alive by the factory (`<Name>ApiFactory`).
- Add/adjust tests with every behavior change. Test names: `Method_condition_expected_result`.

## Commands (run inside `Marketplace/` or `Checkout/`)
```
dotnet build
dotnet test
dotnet run --project src/<Name>.Api     # Marketplace → :5001, Checkout → :5002
```
From the repo root: `docker compose up --build` runs both APIs.

## Conventions
- Keep it minimal: **do not add libraries** (MediatR, AutoMapper, FluentValidation, mocking libs, etc.) without asking the user first.
- Package versions are pinned exactly (EF Core / ASP.NET packages on 10.0.x; 11.x requires net11).
- File-scoped namespaces, primary constructors, `sealed` classes by default, records for DTOs.
- Nullable reference types are enabled; keep the build warning-free.

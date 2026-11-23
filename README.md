# Claude Code Learning – Buy & Checkout samples

Two small ASP.NET Core (.NET 10) sample APIs built with Clean Architecture. They are a deliberately simple starting point to grow in later sessions.

| Project | What it does | Local port |
|---|---|---|
| **Marketplace** | Sellers register and list products (name, price, stock). Buyers browse the catalog. | `http://localhost:5001` |
| **Checkout** | A shopping cart: add/remove items, check out into an order, then pay or cancel it. | `http://localhost:5002` |

The two projects are independent for now: Checkout receives product id/name/price in the request instead of calling Marketplace.

## Layout (same for both)

```
<Project>/
  src/
    <Project>.Domain/          entities, value objects, business rules (no dependencies)
    <Project>.Application/     use-case services, repository interfaces, request/response DTOs
    <Project>.Infrastructure/  EF Core + SQLite: DbContext, mappings, repositories
    <Project>.Api/             Minimal API endpoints, error handling, Program.cs, Dockerfile
  tests/
    <Project>.UnitTests/        Domain and Application tests (hand-written fakes)
    <Project>.IntegrationTests/ Full HTTP tests via WebApplicationFactory + in-memory SQLite
```

Dependencies point inward: `Api → Application + Infrastructure`, `Infrastructure → Application`, `Application → Domain`.

## Endpoints

**Marketplace**
- `POST /sellers` · `GET /sellers/{id}`
- `POST /products` · `GET /products` · `GET /products/{id}`

**Checkout**
- `POST /carts` · `GET /carts/{id}`
- `POST /carts/{id}/items` · `DELETE /carts/{id}/items/{productId}`
- `POST /carts/{id}/checkout` → creates an order
- `GET /orders/{id}` · `POST /orders/{id}/pay` · `POST /orders/{id}/cancel`
- `GET /shipments/{id}` · `GET /shipments/by-number/{number}` (format `SHP-000000`, case-insensitive)

Business-rule errors return `400`, missing resources `404`, both as ProblemDetails JSON. Each Api project has a `.http` file with sample requests.

**API docs (Development only):** interactive Scalar page at `/scalar` (e.g. `http://localhost:5001/scalar`), raw OpenAPI document at `/openapi/v1.json`.

## Run

```powershell
cd Marketplace
dotnet test
dotnet run --project src/Marketplace.Api
```

Same for `Checkout`. The SQLite file (`marketplace.db` / `checkout.db`) is created on first start.

### Sample data
A new database comes with sample data, so you can explore right away:
- **Marketplace:** 3 sellers (Tech Store, Home Goods, Book Nook) and 8 products, one of them out of stock.
- **Checkout:** one open cart, plus two checked-out carts with their orders (one **Paid**, one **Pending**). They use the same product ids, names and prices as the Marketplace data.

Ids are fixed (see `Infrastructure/Persistence/SeedData.cs`) and used in the `.http` files. Sample data is only added when the database is created, so delete the `.db` file to reset it.

### Docker

```powershell
docker compose up --build
```

Runs both APIs (Marketplace on 5001, Checkout on 5002) with their SQLite files on named volumes.

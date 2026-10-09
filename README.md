# WeShopAlot

A complete e-commerce reference application: an **ASP.NET Core Web API (.NET 10)** with a product catalog, a Redis-backed basket, JWT sign-in, orders, and Stripe card payments, plus an **Angular 22** client that uses it. It is the reference app for the course **Angular and .NET Web APIs** — https://mikecostarella.github.io/CS_AngularAndDotNetAPI/ — where every module reads part of it, including the parts it gets wrong.

## What's in the solution (`Solution/`)

| Project | What it holds |
|---|---|
| `WeShopAlot..WebAPI` | Controllers, middleware, error types, the `[Cached]` attribute, Swagger, `Program.cs`; serves the built Angular app from `wwwroot` |
| `WeShopAlot.Data` | EF Core context, entities, migrations, repositories, query specifications |
| `WeShopAlot.Infrastructure.Services` | Orders, Stripe payments, JWT tokens, Redis response cache |
| `WeShopAlot.Shared` | DTOs shared by the API and its .NET clients |
| `WeShopAlot.Data.ConsoleApp` | Creates the database and loads the JSON seed data |
| `Client` | The Angular client |
| `WeShopAlot.UI.ClientMVC`, `WeShopAlot.UI.ClientWPF` | Two more clients of the same API |
| `WeShopAlot.Testing.*` | Tests; `Testing.WebApis` runs the whole API in memory with WebApplicationFactory |
| `Deployment`, `docker-compose.yml` | ARM templates for Azure; Redis for local development |

## Prerequisites

- .NET 10 SDK
- Node.js 22.22.3+ or 24.15+ (required by Angular 22)
- Docker Desktop (for Redis)
- SQL Server LocalDB or SQL Server (PostgreSQL also works; see `Settings:DbServerType`)

## Secrets

No keys, passwords, or connection strings are committed. Set them with `dotnet user-secrets` in the API project (PowerShell):

```
cd Solution\WeShopAlot..WebAPI
dotnet user-secrets set "Settings:DbServerType" "SQLServer"
dotnet user-secrets set "ConnectionStrings:WeShopAlotSQLConnection" "Server=(localdb)\MSSQLLocalDB;Database=WeShopAlot;Trusted_Connection=true;TrustServerCertificate=true"
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379"
dotnet user-secrets set "Token:Key" "<at least 64 random characters>"
dotnet user-secrets set "StripeSettings:SecretKey" "sk_test_..."
dotnet user-secrets set "StripeSettings:WhSecret" "whsec_..."
dotnet user-secrets set "AutoMapper:LicenseKey" "<optional; AutoMapper logs a warning without one>"
```

The Stripe publishable key (`pk_test_...`) is public by design and lives in `appsettings.json` and `Client/src/environments/`. Keys that appeared in this repository's history before October 2026 are retired; never reuse one.

## Run it

```
cd Solution
docker compose up -d                                   # Redis on 6379 (viewer on http://localhost:8081)
dotnet run --project WeShopAlot.Data.ConsoleApp        # first time: create and seed the database (uses its own user secrets)
cd Client
npm install
npx ng build                                           # builds into the API's wwwroot
cd ..
dotnet run --project "WeShopAlot..WebAPI" --launch-profile https
```

Browse to https://localhost:7244 for the app, or https://localhost:7244/swagger for the API. For client development, run `npx ng serve` in `Client` and browse to https://localhost:4200.

## Test it

```
cd Solution
dotnet test                       # API integration tests: no database or Redis needed
cd Client
npx ng test --watch=false         # Angular unit tests (Vitest)
```

## History

Written in 2023–2024 on .NET 8 with an Angular 15 client; upgraded in October 2026 to .NET 10 and Angular 22. The tests added for the upgrade found several long-standing bugs (client routes that did not match the API, tokens without an issuer, broken address endpoints, a shipping rounding error); the course's WeShopAlot page describes each one.

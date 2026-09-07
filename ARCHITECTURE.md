# CCMSApp Architecture

This document describes the architecture and conventions used in the CCMSApp .NET 10 REST API boilerplate.

## Clean Architecture Layers

```
┌─────────────────────────────────────────────┐
│              CCMSApp.WebApi                 │
│         Controllers, Middleware, DI         │
├─────────────────────────────────────────────┤
│           CCMSApp.Application               │
│   Commands, Queries, Validators, DTOs       │
├─────────────────────────────────────────────┤
│           CCMSApp.Infrastructure            │
│  Dapper Repositories, SQL, Health Checks    │
├─────────────────────────────────────────────┤
│              CCMSApp.Core                   │
│   Entities, Abstractions, Common Types      │
└─────────────────────────────────────────────┘
```

Dependency direction is inward: **WebApi** → **Application** → **Infrastructure/Core**. The Core layer has no dependencies on other layers.

## Request Flow

```
HTTP Request
    │
    ▼
Swagger / API Versioning
    │
    ▼
CorrelationIdMiddleware  ── adds X-Correlation-ID
    │
    ▼
GlobalExceptionMiddleware  ── catches unhandled exceptions
    │
    ▼
Controller (UsersController)
    │
    ▼
MediatR Command / Query
    │
    ▼
Handler + FluentValidation
    │
    ▼
Repository Interface (Core)
    │
    ▼
Dapper Repository (Infrastructure)
    │
    ▼
Azure SQL Server
```

## Project Responsibilities

### Core

- Domain entities (`User`)
- Repository abstractions (`IUserRepository`, `IDbConnectionFactory`)
- Common types (`Result`, `PagedList`)
- Domain-specific exceptions (`NotFoundException`, `ValidationException`)

### Application

- Use cases organized as MediatR commands and queries
- Input validation with FluentValidation
- Object mapping with Mapster
- Pipeline behaviors for cross-cutting concerns (validation, logging)

### Infrastructure

- Dapper repository implementations
- SQL connection factory
- Health checks
- External services (date/time provider)

### WebApi

- HTTP controllers
- Request/response models
- Global exception handling
- Correlation ID middleware
- Swagger/OpenAPI configuration
- API versioning
- Response envelope filter (`ApiResponse<T>`)

## Dapper Conventions

- Use parameterized SQL or stored procedures.
- Never use `SELECT *`; explicitly list columns.
- Pass `CancellationToken` through all async calls.
- Open connections per operation; do not hold them across requests.
- Map SQL columns to entity properties by name matching.

## Repository Pattern

Repository interfaces live in **Core** so the Application layer depends on abstractions, not concrete data access. Implementations live in **Infrastructure** and are registered with dependency injection.

## Validation

- FluentValidation validators are co-located with commands.
- `ValidationBehavior` runs validators before the handler executes.
- Domain validation can throw `ValidationException` from handlers.

## Exception Handling

The `GlobalExceptionMiddleware` maps exceptions to RFC 7807 `ProblemDetails`:

| Exception | HTTP Status |
|-----------|-------------|
| `NotFoundException` | 404 |
| `ValidationException` | 400 |
| All others | 500 |

In Development mode, the original exception message is included. In Production, a generic message is returned.

## API Versioning

URL path versioning is used: `/api/v1/users`. The default version is `1.0` and unspecified versions default to it.

## Response Envelope

Successful JSON responses are wrapped by `ApiResponseFilter`:

```json
{
  "data": { ... },
  "correlationId": "...",
  "success": true
}
```

`NoContentResult` and error responses are not wrapped.

## Logging

Serilog is configured in `Program.cs`. Every request is logged with:

- Correlation ID
- Request path
- Status code
- Elapsed milliseconds

Use structured log templates instead of string interpolation.

## Health Checks

- `/health/live` — liveness probe; always returns healthy if the app is running.
- `/health/ready` — readiness probe; checks Azure SQL connectivity.

## Database Migrations

Migration scripts are stored in `scripts/` and applied manually. This keeps the boilerplate simple and CI/CD-agnostic. For larger projects, consider adopting DbUp or FluentMigrator.

## Naming Conventions

- Projects: `CCMSApp.{Layer}`
- Commands: `{Action}{Entity}Command`
- Queries: `Get{Entity}ByIdQuery`, `Search{Entities}Query`
- Handlers: `{CommandName}Handler`
- Validators: `{CommandName}Validator`
- Repositories: `{Entity}Repository` / `I{Entity}Repository`

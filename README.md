# CCMSApp

A production-ready **.NET 10** REST API boilerplate built with **Clean Architecture**, **Dapper**, and **Microsoft Azure SQL Server**.

## Technology Stack

- **.NET 10** Web API
- **Dapper** for high-performance data access
- **Microsoft.Data.SqlClient** for Azure SQL connectivity
- **MediatR** for CQRS-style command and query handlers
- **FluentValidation** for request validation
- **Mapster** for object mapping
- **Serilog** for structured logging
- **Swashbuckle.AspNetCore** for OpenAPI/Swagger documentation
- **Asp.Versioning** for API versioning

## Solution Structure

```
CCMSApp/
├── scripts/                 -- Database migration scripts
├── src/
│   ├── CCMSApp.Core/        -- Domain entities, abstractions, common types
│   ├── CCMSApp.Application/ -- Use cases, commands, queries, validation
│   ├── CCMSApp.Infrastructure/ -- Dapper repositories, SQL health checks, services
│   └── CCMSApp.WebApi/      -- Controllers, middleware, Swagger, DI composition
├── .editorconfig
├── Directory.Build.props
└── Directory.Packages.props
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- An Azure SQL database or a local SQL Server instance

### 1. Clone and restore

```powershell
dotnet restore
```

### 2. Configure the database

Update `src/CCMSApp.WebApi/appsettings.json` (or user secrets) with your Azure SQL connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=CCMSAppDb;User ID=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=False;"
}
```

For local development, `appsettings.Development.json` defaults to `(localdb)\MSSQLLocalDB`.

### 3. Run the database script

Execute `scripts/001_InitialSchema.sql` against your target database using Azure Data Studio, SSMS, or `sqlcmd`.

### 4. Run the API

```powershell
dotnet run --project src/CCMSApp.WebApi
```

The API will start on `https://localhost:7001` (or similar). Swagger UI is available at the root URL in Development mode.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/users` | Get paginated users |
| GET | `/api/v1/users/search` | Search users by name or email |
| GET | `/api/v1/users/{id}` | Get user by ID |
| POST | `/api/v1/users` | Create a new user |
| PUT | `/api/v1/users/{id}` | Update a user |
| DELETE | `/api/v1/users/{id}` | Delete a user |
| GET | `/health/live` | Liveness probe |
| GET | `/health/ready` | Readiness probe (checks Azure SQL) |

### Example Request

```bash
curl -X POST https://localhost:7001/api/v1/users \
  -H "Content-Type: application/json" \
  -H "X-Correlation-ID: abc-123" \
  -d '{"name":"Jane Doe","email":"jane.doe@example.com"}'
```

### Example Response

```json
{
  "data": {
    "id": "a1b2c3d4-e5f6-7890-abcd-ef0123456789",
    "name": "Jane Doe",
    "email": "jane.doe@example.com",
    "createdAt": "2026-07-31T12:00:00Z",
    "updatedAt": null
  },
  "correlationId": "abc-123",
  "success": true
}
```

## Configuration

Key settings in `appsettings.json`:

| Section | Purpose |
|---------|---------|
| `ConnectionStrings:DefaultConnection` | Azure SQL connection string |
| `Database:CommandTimeoutSeconds` | SQL command timeout |
| `Serilog` | Logging configuration |

Use `dotnet user-secrets` for sensitive values in development:

```powershell
cd src/CCMSApp.WebApi
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

## Authentication

This boilerplate ships with anonymous endpoints. A `Bearer` security definition is included in Swagger so you can easily add JWT authentication later by:

1. Installing `Microsoft.AspNetCore.Authentication.JwtBearer`.
2. Configuring JWT validation in `Program.cs`.
3. Adding `[Authorize]` attributes to controllers/actions.

## Azure SQL Authentication Notes

The default connection factory uses SQL authentication via the connection string. For production Azure deployments, consider:

- **Managed Identity** with `Azure.Identity.DefaultAzureCredential`
- **Microsoft Entra ID** authentication
- **Azure Key Vault** for connection string secrets

## Health Checks

- `/health/live` returns `Healthy` when the application is running.
- `/health/ready` verifies Azure SQL connectivity and returns detailed JSON.

Map these endpoints to your Azure App Service health probes or Kubernetes liveness/readiness probes.

## Logging

Structured logs are written to the console by default. Each request is enriched with:

- `CorrelationId`
- `MachineName`
- `ThreadId`
- Request path and elapsed time

## Next Steps

- Add JWT or Entra ID authentication.
- Add integration tests with Testcontainers.MsSql.
- Add a CI/CD pipeline (GitHub Actions or Azure DevOps).
- Introduce domain events for cross-cutting concerns.

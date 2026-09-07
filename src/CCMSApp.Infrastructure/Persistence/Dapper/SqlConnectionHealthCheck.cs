using CCMSApp.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Common;

namespace CCMSApp.Infrastructure.Persistence.Dapper;

/// <summary>
/// Health check that verifies connectivity to Azure SQL Server.
/// </summary>
public sealed class SqlConnectionHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlConnectionHealthCheck(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy("Azure SQL is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Azure SQL is unreachable.", ex);
        }
    }
}

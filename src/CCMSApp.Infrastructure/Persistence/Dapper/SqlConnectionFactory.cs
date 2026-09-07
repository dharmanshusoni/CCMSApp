using CCMSApp.Core.Interfaces;
using CCMSApp.Infrastructure.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace CCMSApp.Infrastructure.Persistence.Dapper;

/// <summary>
/// Creates SQL Server connections for Dapper using the configured connection string.
/// </summary>
public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly DatabaseSettings _settings;
    private readonly string _connectionString;

    public SqlConnectionFactory(IOptions<DatabaseSettings> settings, IConfiguration configuration)
    {
        _settings = settings.Value;
        _connectionString = configuration.GetConnectionString(_settings.ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{_settings.ConnectionStringName}' is not configured.");
    }

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}

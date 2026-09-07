using System.Data.Common;

namespace CCMSApp.Core.Interfaces;

/// <summary>
/// Creates database connections for Dapper repositories.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a new closed database connection.
    /// </summary>
    DbConnection CreateConnection();

    /// <summary>
    /// Creates a new database connection and opens it asynchronously.
    /// </summary>
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}

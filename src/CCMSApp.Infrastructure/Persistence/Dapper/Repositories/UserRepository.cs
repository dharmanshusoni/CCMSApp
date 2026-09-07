using CCMSApp.Core.Common;
using CCMSApp.Core.Entities;
using CCMSApp.Core.Interfaces;
using CCMSApp.Core.Interfaces.Repositories;
using Dapper;
using System.Data.Common;

namespace CCMSApp.Infrastructure.Persistence.Dapper.Repositories;

/// <summary>
/// Dapper implementation of the user repository.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, Name, Email, CreatedAt, UpdatedAt
            FROM Users
            WHERE Id = @Id";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<User>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, Name, Email, CreatedAt, UpdatedAt
            FROM Users
            WHERE Email = @Email";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<User>(
            new CommandDefinition(sql, new { Email = email.ToLowerInvariant() }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, Name, Email, CreatedAt, UpdatedAt
            FROM Users
            ORDER BY CreatedAt DESC";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var users = await connection.QueryAsync<User>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return users.ToList();
    }

    public async Task<PagedList<User>> SearchAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        var whereClause = string.IsNullOrWhiteSpace(searchTerm)
            ? string.Empty
            : "WHERE Name LIKE @SearchTerm OR Email LIKE @SearchTerm";

        var countSql = $@"
            SELECT COUNT(*)
            FROM Users
            {whereClause}";

        var dataSql = $@"
            SELECT Id, Name, Email, CreatedAt, UpdatedAt
            FROM Users
            {whereClause}
            ORDER BY CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var parameters = new
        {
            SearchTerm = $"%{searchTerm?.Trim() ?? string.Empty}%",
            Offset = (pageNumber - 1) * pageSize,
            PageSize = pageSize
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var totalCount = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));

        var users = await connection.QueryAsync<User>(
            new CommandDefinition(dataSql, parameters, cancellationToken: cancellationToken));

        return new PagedList<User>(users.ToList(), totalCount, pageNumber, pageSize);
    }

    public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO Users (Id, Name, Email, CreatedAt, UpdatedAt)
            VALUES (@Id, @Name, @Email, @CreatedAt, @UpdatedAt)";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));

        return user.Id;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE Users
            SET Name = @Name,
                Email = @Email,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            DELETE FROM Users
            WHERE Id = @Id";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM Users
            WHERE Email = @Email";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, new { Email = email.ToLowerInvariant() }, cancellationToken: cancellationToken));

        return count > 0;
    }
}

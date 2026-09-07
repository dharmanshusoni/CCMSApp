using CCMSApp.Core.Common;
using CCMSApp.Core.Entities;

namespace CCMSApp.Core.Interfaces.Repositories;

/// <summary>
/// Repository abstraction for user accounts.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PagedList<User>> SearchAsync(string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}

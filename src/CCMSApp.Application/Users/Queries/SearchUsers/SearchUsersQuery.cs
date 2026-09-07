using CCMSApp.Core.Common;
using MediatR;

namespace CCMSApp.Application.Users.Queries.SearchUsers;

/// <summary>
/// Query to search users by name or email.
/// </summary>
public sealed record SearchUsersQuery(
    string? SearchTerm,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedList<UserDto>>;

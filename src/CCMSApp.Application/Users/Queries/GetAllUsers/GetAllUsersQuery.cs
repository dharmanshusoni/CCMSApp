using CCMSApp.Core.Common;
using MediatR;

namespace CCMSApp.Application.Users.Queries.GetAllUsers;

/// <summary>
/// Query to retrieve a paginated list of all users.
/// </summary>
public sealed record GetAllUsersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedList<UserDto>>;

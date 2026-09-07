using MediatR;

namespace CCMSApp.Application.Users.Queries.GetUserById;

/// <summary>
/// Query to retrieve a single user by identifier.
/// </summary>
public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

using MediatR;

namespace CCMSApp.Application.Users.Commands.UpdateUser;

/// <summary>
/// Command to update an existing user account.
/// </summary>
public sealed record UpdateUserCommand(Guid Id, string Name, string Email) : IRequest<UserDto>;

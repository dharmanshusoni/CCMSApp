using MediatR;

namespace CCMSApp.Application.Users.Commands.CreateUser;

/// <summary>
/// Command to create a new user account.
/// </summary>
public sealed record CreateUserCommand(string Name, string Email) : IRequest<UserDto>;

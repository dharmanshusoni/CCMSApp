using MediatR;

namespace CCMSApp.Application.Users.Commands.DeleteUser;

/// <summary>
/// Command to delete a user account.
/// </summary>
public sealed record DeleteUserCommand(Guid Id) : IRequest;

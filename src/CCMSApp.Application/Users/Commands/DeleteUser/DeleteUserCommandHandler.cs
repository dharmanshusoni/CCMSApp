using CCMSApp.Core.Common.Exceptions;
using CCMSApp.Core.Interfaces.Repositories;
using MediatR;

namespace CCMSApp.Application.Users.Commands.DeleteUser;

/// <summary>
/// Handles deletion of a user account.
/// </summary>
public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("User", request.Id);

        await _userRepository.DeleteAsync(user.Id, cancellationToken);
    }
}

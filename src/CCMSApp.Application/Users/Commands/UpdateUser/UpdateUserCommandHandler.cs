using CCMSApp.Core.Common.Exceptions;
using CCMSApp.Core.Interfaces;
using CCMSApp.Core.Interfaces.Repositories;
using MapsterMapper;
using MediatR;

namespace CCMSApp.Application.Users.Commands.UpdateUser;

/// <summary>
/// Handles updates to an existing user account.
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("User", request.Id);

        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null && existing.Id != request.Id)
            throw new ValidationException($"A user with email '{request.Email}' already exists.");

        user.Update(request.Name, request.Email, _dateTimeProvider);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}

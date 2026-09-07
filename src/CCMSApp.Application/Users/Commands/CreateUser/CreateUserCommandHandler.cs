using CCMSApp.Core.Common.Exceptions;
using CCMSApp.Core.Entities;
using CCMSApp.Core.Interfaces;
using CCMSApp.Core.Interfaces.Repositories;
using MapsterMapper;
using MediatR;

namespace CCMSApp.Application.Users.Commands.CreateUser;

/// <summary>
/// Handles creation of a new user account.
/// </summary>
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ValidationException($"A user with email '{request.Email}' already exists.");

        var user = User.Create(request.Name, request.Email, _dateTimeProvider);

        await _userRepository.AddAsync(user, cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}

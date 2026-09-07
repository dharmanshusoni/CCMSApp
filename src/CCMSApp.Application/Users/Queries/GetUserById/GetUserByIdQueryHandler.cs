using CCMSApp.Core.Interfaces.Repositories;
using MapsterMapper;
using MediatR;

namespace CCMSApp.Application.Users.Queries.GetUserById;

/// <summary>
/// Handles retrieval of a single user by identifier.
/// </summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        return user is null ? null : _mapper.Map<UserDto>(user);
    }
}

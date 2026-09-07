using CCMSApp.Core.Common;
using CCMSApp.Core.Interfaces.Repositories;
using MapsterMapper;
using MediatR;

namespace CCMSApp.Application.Users.Queries.GetAllUsers;

/// <summary>
/// Handles retrieval of a paginated list of all users.
/// </summary>
public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.SearchAsync(
            null,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var dtos = _mapper.Map<List<UserDto>>(users.ToList());

        return new PagedList<UserDto>(
            dtos,
            users.TotalCount,
            users.CurrentPage,
            users.PageSize);
    }
}

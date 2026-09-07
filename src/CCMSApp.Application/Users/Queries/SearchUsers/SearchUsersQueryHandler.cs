using CCMSApp.Core.Common;
using CCMSApp.Core.Interfaces.Repositories;
using MapsterMapper;
using MediatR;

namespace CCMSApp.Application.Users.Queries.SearchUsers;

/// <summary>
/// Handles user search queries.
/// </summary>
public sealed class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, PagedList<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<UserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.SearchAsync(
            request.SearchTerm,
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

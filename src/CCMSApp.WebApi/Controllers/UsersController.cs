using Asp.Versioning;
using CCMSApp.Application.Users;
using CCMSApp.Application.Users.Commands.CreateUser;
using CCMSApp.Application.Users.Commands.DeleteUser;
using CCMSApp.Application.Users.Commands.UpdateUser;
using CCMSApp.Application.Users.Queries.GetAllUsers;
using CCMSApp.Application.Users.Queries.GetUserById;
using CCMSApp.Application.Users.Queries.SearchUsers;
using CCMSApp.WebApi.Common.Mappings;
using CCMSApp.WebApi.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace CCMSApp.WebApi.Controllers;

[ApiVersion("1.0")]
public class UsersController : ApiControllerBase
{
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paginated list of users.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllUsersQuery(pageNumber, pageSize);
        var result = await Mediator.Send(query);
        return Ok(_mapper.MapPagedList<UserDto, UserResponse>(result));
    }

    /// <summary>
    /// Searches users by name or email.
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchUsersQuery(searchTerm, pageNumber, pageSize);
        var result = await Mediator.Send(query);
        return Ok(_mapper.MapPagedList<UserDto, UserResponse>(result));
    }

    /// <summary>
    /// Gets a user by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await Mediator.Send(new GetUserByIdQuery(id));

        if (user is null)
            return NotFound();

        return Ok(_mapper.Map<UserResponse>(user));
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, _mapper.Map<UserResponse>(result));
    }

    /// <summary>
    /// Updates an existing user account.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var command = _mapper.Map<UpdateUserCommand>(request) with { Id = id };
        var result = await Mediator.Send(command);
        return Ok(_mapper.Map<UserResponse>(result));
    }

    /// <summary>
    /// Deletes a user account.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }
}

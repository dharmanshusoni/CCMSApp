using CCMSApp.Application.Users;
using CCMSApp.Application.Users.Commands.CreateUser;
using CCMSApp.Application.Users.Commands.UpdateUser;
using CCMSApp.WebApi.Models;
using Mapster;

namespace CCMSApp.WebApi.Common.Mappings;

/// <summary>
/// Mapster mapping configuration for WebApi models.
/// </summary>
public class WebApiMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, CreateUserCommand>();
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>();
        config.NewConfig<UserDto, UserResponse>();
    }
}

using CCMSApp.Application.Users;
using CCMSApp.Core.Entities;
using Mapster;

namespace CCMSApp.Application.Common.Mappings;

/// <summary>
/// Mapster mapping configuration between domain entities and DTOs.
/// </summary>
public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>();
    }
}

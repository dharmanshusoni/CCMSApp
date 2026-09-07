namespace CCMSApp.Application.Users;

/// <summary>
/// Data-transfer object representing a user account.
/// </summary>
public sealed record UserDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

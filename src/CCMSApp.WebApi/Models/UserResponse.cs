namespace CCMSApp.WebApi.Models;

/// <summary>
/// Response model for a user account.
/// </summary>
public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

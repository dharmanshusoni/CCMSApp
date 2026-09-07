namespace CCMSApp.WebApi.Models;

/// <summary>
/// Request to create a new user account.
/// </summary>
public sealed record CreateUserRequest(string Name, string Email);

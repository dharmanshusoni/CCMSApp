namespace CCMSApp.WebApi.Models;

/// <summary>
/// Request to update an existing user account.
/// </summary>
public sealed record UpdateUserRequest(string Name, string Email);

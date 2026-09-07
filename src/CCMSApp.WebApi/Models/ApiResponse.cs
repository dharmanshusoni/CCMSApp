namespace CCMSApp.WebApi.Models;

/// <summary>
/// Standard API response envelope.
/// </summary>
/// <typeparam name="T">The type of the response data.</typeparam>
public sealed record ApiResponse<T>(T Data, string CorrelationId, bool Success = true);

namespace CCMSApp.Core.Interfaces;

/// <summary>
/// Provides the current UTC date and time.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

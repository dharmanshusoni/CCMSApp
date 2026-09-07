using CCMSApp.Core.Interfaces;

namespace CCMSApp.Infrastructure.Services;

/// <summary>
/// Default implementation of the date/time provider using system UTC time.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

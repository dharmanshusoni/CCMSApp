using CCMSApp.Core.Interfaces;

namespace CCMSApp.Core.Entities;

/// <summary>
/// A user account in the system.
/// </summary>
public class User
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private User()
    {
    }

    public static User Create(string name, string email, IDateTimeProvider dateTimeProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return new User
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            CreatedAt = dateTimeProvider.UtcNow
        };
    }

    public void Update(string name, string email, IDateTimeProvider dateTimeProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        UpdatedAt = dateTimeProvider.UtcNow;
    }
}

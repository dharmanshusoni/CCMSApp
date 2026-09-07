using CCMSApp.Core.Interfaces;
using CCMSApp.Core.Interfaces.Repositories;
using CCMSApp.Infrastructure.Persistence.Dapper;
using CCMSApp.Infrastructure.Persistence.Dapper.Repositories;
using CCMSApp.Infrastructure.Services;
using CCMSApp.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CCMSApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(
            configuration.GetSection(DatabaseSettings.SectionName));

        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddHealthChecks()
            .AddCheck<SqlConnectionHealthCheck>(
                "azure-sql",
                HealthStatus.Unhealthy,
                new[] { "db", "ready" });

        return services;
    }
}

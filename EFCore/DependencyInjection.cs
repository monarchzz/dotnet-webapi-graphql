using EFCore.Common;
using EFCore.ConnectionString;
using EFCore.Context;
using EFCore.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EFCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEFCore(this IServiceCollection services)
    {
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddDbContext<TenantDbContext>()
            .AddDbContext<AppDbContext>()
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<ApplicationDbSeeder>()
            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            .AddTransient<IConnectionStringValidator, ConnectionStringValidator>();

        return services;
    }

    public static async Task InitializeDatabasesAsync(this IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }
}
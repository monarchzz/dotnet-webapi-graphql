using Domain.Entities;
using EFCore.Context;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Multitenancy;
using Shared.Provider;

namespace EFCore.Initialization;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(TenantDbContext tenantDbContext, IServiceProvider serviceProvider
    )
    {
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        await InitializeTenantDbAsync(cancellationToken);

        // init ten
        // foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
        // {
        //     await InitializeApplicationDbForTenantAsync(user, tenant,
        //         cancellationToken);
        // }
    }

    public async Task InitializeApplicationDbForTenantAsync(User user, VHNTenantInfo tenant,
        CancellationToken cancellationToken)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then set current tenant so the right connection string is used
        _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
            .MultiTenantContext = new MultiTenantContext<VHNTenantInfo>()
        {
            TenantInfo = tenant
        };

        // Then run the initialization in the new scope
        await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
            .InitializeAsync(user, cancellationToken);
    }

    private async Task InitializeTenantDbAsync(CancellationToken cancellationToken)
    {
        if ((await _tenantDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await _tenantDbContext.Database.MigrateAsync(cancellationToken);
        }

        await SeedTenantAsync(cancellationToken);
    }

    // Seed the tenant database with some initial data
    private Task SeedTenantAsync(CancellationToken cancellationToken)
    {
        // if (await _tenantDbContext.TenantInfo.FindAsync(new object?[] {MultitenancyConstants.Root.Id},
        //         cancellationToken: cancellationToken) is null)
        // {
        //     var rootTenant = new VHNTenantInfo
        //     {
        //         Id = MultitenancyConstants.Root.Id,
        //         Identifier = MultitenancyConstants.Root.Id,
        //         Name = MultitenancyConstants.Root.Name,
        //         ConnectionString = string.Empty,
        //         AdminEmail = MultitenancyConstants.Root.EmailAddress,
        //         IsActive = true,
        //         ValidUpto = DateTimeProvider.MaxDate,
        //     };
        //
        //     _tenantDbContext.TenantInfo.Add(rootTenant);
        //
        //     await _tenantDbContext.SaveChangesAsync(cancellationToken);
        // }

        return Task.CompletedTask;
    }
}
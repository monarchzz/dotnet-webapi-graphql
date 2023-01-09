using Domain.Entities;
using EFCore.Initialization;
using Finbuckle.MultiTenant;
using Shared.Provider;

namespace API.Resolvers.Tenants;

[ExtendObjectType(OperationTypeNames.Query)]
public class TenantQueries
{
    public async Task<bool> GetTenant(CancellationToken cancellationToken,
        [Service]
        IDatabaseInitializer databaseInitializer,
        [Service]
        IMultiTenantStore<VHNTenantInfo> tenantStore)
    {
        var tenant = new VHNTenantInfo
        {
            Id = "demo",
            Identifier = "demo",
            Name = "demo",
            ConnectionString = "Data Source=.;Initial Catalog=DEMOVHN;Integrated Security=True",
            AdminEmail = "d",
            IsActive = true,
            ValidUpto = DateTimeProvider.MaxDate
        };
        await tenantStore.TryAddAsync(tenant);

        try
        {
            await databaseInitializer.InitializeApplicationDbForTenantAsync("demo", tenant, cancellationToken);
        }
        catch (Exception e)
        {
            await tenantStore.TryRemoveAsync("demo");
            throw;
        }

        return true;
    }
}
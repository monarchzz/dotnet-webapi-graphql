using API.Resolvers.Tenants.Dtos;
using Domain.Entities;
using EFCore.ConnectionString;
using Finbuckle.MultiTenant;
using Mapster;

namespace API.Resolvers.Tenants;

[ExtendObjectType(OperationTypeNames.Query)]
public class TenantQueries
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<TenantPayload>> GetTenants([Service()] IConnectionStringSecurer csSecurer,
        [Service(ServiceKind.Synchronized)] IMultiTenantStore<VHNTenantInfo> tenantStore)
    {
        var tenants = await tenantStore.GetAllAsync();
        var tenantsPayload = tenants.Adapt<List<TenantPayload>>();

        tenantsPayload.ForEach(t => t.ConnectionString = csSecurer.MakeSecure(t.ConnectionString) ?? "");

        return tenantsPayload;
    }

    public async Task<TenantPayload?> GetTenant(string id,
        [Service()] IConnectionStringSecurer csSecurer,
        [Service(ServiceKind.Synchronized)] IMultiTenantStore<VHNTenantInfo> tenantStore)
    {
        var tenant = await tenantStore.TryGetAsync(id);

        var tenantPayload = tenant?.Adapt<TenantPayload>();
        if (tenantPayload is not null)
        {
            tenantPayload.ConnectionString = csSecurer.MakeSecure(tenant!.ConnectionString) ?? "";
        }

        return tenantPayload;
    }
}
using API.Errors;
using API.Resolvers.Tenants.Dtos;
using Domain.Entities;
using EFCore.ConnectionString;
using EFCore.Initialization;
using Finbuckle.MultiTenant;
using Mapster;
using Shared.Authentication.Interface;
using Shared.Provider;

namespace API.Resolvers.Tenants;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class TenantMutations
{
    public async Task<TenantPayload> CreateTenant(CreateTenantInput input,
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IMultiTenantStore<VHNTenantInfo> tenantStore,
        [Service()] IDatabaseInitializer dbInitializer,
        [Service()] IConnectionStringSecurer csSecurer,
        [Service()] IPasswordHelper passwordHelper,
        [Service()] IConnectionStringGenerator csGenerator)
    {
        if (await tenantStore.TryGetByIdentifierAsync(input.Identifier) != null)
            throw AppErrors.Tenant.AlreadyExists;

        var tenant = new VHNTenantInfo
        {
            Id = input.Identifier,
            Identifier = input.Identifier,
            Name = input.TenantName,
            ConnectionString = csGenerator.Generate(input.Identifier),
            AdminEmail = input.Email,
            ValidUpto = DateTimeProvider.MaxDate,
        };
        var user = new User
        {
            Id = default,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            Password = passwordHelper.HashPassword(input.Password),
        };

        await tenantStore.TryAddAsync(tenant);

        try
        {
            await dbInitializer.InitializeApplicationDbForTenantAsync(user, tenant, cancellationToken);
        }
        catch
        {
            await tenantStore.TryRemoveAsync(tenant.Id);
            throw;
        }

        var tenantPayload = tenant.Adapt<TenantPayload>();
        tenantPayload.ConnectionString = csSecurer.MakeSecure(tenant!.ConnectionString) ?? "";

        return tenantPayload;
    }

    // public async Task<bool> Activate(string id,
    //     [Service(ServiceKind.Synchronized)] IMultiTenantStore<VHNTenantInfo> tenantStore)
    // {
    //     var tenant = await tenantStore.TryGetAsync(id);
    //     if (tenant == null) throw AppErrors.Tenant.NotExists;
    //
    //     tenant.Activate();
    //
    //     await tenantStore.TryUpdateAsync(tenant);
    //
    //     return true;
    // }
    //
    // public async Task<bool> Deactivate(string id,
    //     [Service(ServiceKind.Synchronized)] IMultiTenantStore<VHNTenantInfo> tenantStore)
    // {
    //     var tenant = await tenantStore.TryGetAsync(id);
    //     if (tenant == null) throw AppErrors.Tenant.NotExists;
    //
    //     tenant.Deactivate();
    //
    //     await tenantStore.TryUpdateAsync(tenant);
    //
    //     return true;
    // }
    //
    // public async Task<bool> UpdateSubscription(UpdateSubscriptionInput input,
    //     CancellationToken cancellationToken,
    //     [Service(ServiceKind.Synchronized)] IMultiTenantStore<VHNTenantInfo> tenantStore)
    // {
    //     var tenant = await tenantStore.TryGetAsync(input.Id);
    //     if (tenant == null) throw AppErrors.Tenant.NotExists;
    //
    //     tenant.SetValidity(input.ExpiryDate);
    //
    //     await tenantStore.TryUpdateAsync(tenant);
    //
    //     return true;
    // }
}
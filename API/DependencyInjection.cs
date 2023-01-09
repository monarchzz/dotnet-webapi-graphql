using API.Resolvers;
using Domain.Entities;
using EFCore.Context;
using Shared.Authorization;
using Shared.Multitenancy;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        return services
            .AddResolvers()
            .AddMultitenancy();
    }

    private static IServiceCollection AddMultitenancy(this IServiceCollection services)
    {
        return services
            .AddMultiTenant<VHNTenantInfo>()
            // .WithClaimStrategy(Claims.Tenant)
            .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
            .WithEFCoreStore<TenantDbContext, VHNTenantInfo>()
            .Services;
    }
}
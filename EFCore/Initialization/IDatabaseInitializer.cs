using Domain.Entities;

namespace EFCore.Initialization;

public interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);

    Task InitializeApplicationDbForTenantAsync(string adminPassword, VHNTenantInfo tenant,
        CancellationToken cancellationToken);
}
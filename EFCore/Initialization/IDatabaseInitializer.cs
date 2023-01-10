using Domain.Entities;

namespace EFCore.Initialization;

public interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);

    Task InitializeApplicationDbForTenantAsync(User user, VHNTenantInfo tenant,
        CancellationToken cancellationToken);
}
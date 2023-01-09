using EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Initialization;

public class ApplicationDbInitializer
{
    private readonly AppDbContext _dbContext;
    private readonly ApplicationDbSeeder _dbSeeder;

    public ApplicationDbInitializer(AppDbContext dbContext, ApplicationDbSeeder dbSeeder)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
    }

    public async Task InitializeAsync(string adminPassword, CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                await _dbSeeder.SeedDatabaseAsync(adminPassword, _dbContext, cancellationToken);
            }
        }
    }
}
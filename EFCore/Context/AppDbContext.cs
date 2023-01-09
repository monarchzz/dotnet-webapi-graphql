using Domain.Entities;
using EFCore.Common;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFCore.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(ITenantInfo? tenantInfo, IOptions<DatabaseSettings> databaseSettings)
    {
        _connectionString = databaseSettings.Value.ConnectionString;
        TenantInfo = tenantInfo;
    }

    private readonly string _connectionString;
    private ITenantInfo? TenantInfo { get; set; }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RoleClaim> RoleClaims => Set<RoleClaim>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // Config enum
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // config connection string
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
        }
        else
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
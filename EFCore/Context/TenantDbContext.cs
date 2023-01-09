using Domain.Entities;
using EFCore.Common;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFCore.Context;

public class TenantDbContext : EFCoreStoreDbContext<VHNTenantInfo>
{
    private readonly string _connectionString;

    public TenantDbContext(DbContextOptions<TenantDbContext> options, IOptions<DatabaseSettings> databaseSettings)
        : base(options)
    {
        _connectionString = databaseSettings.Value.ConnectionString;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VHNTenantInfo>().ToTable("Tenants");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
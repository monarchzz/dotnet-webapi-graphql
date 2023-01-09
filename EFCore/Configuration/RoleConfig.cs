using Domain.Entities;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configuration;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // builder.IsMultiTenant();

        builder.HasKey(role => role.Id);
        builder
            .Property(r => r.Name)
            .HasColumnType("nvarchar(500)");

        builder
            .Property(r => r.Description)
            .HasColumnType("nvarchar(1000)");
    }
}
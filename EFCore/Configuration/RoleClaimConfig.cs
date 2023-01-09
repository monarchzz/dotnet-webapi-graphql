using Domain.Entities;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configuration;

public class RoleClaimConfig : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        // builder.IsMultiTenant();

        builder.HasKey(rc => rc.Id);
        builder
            .Property(rc => rc.Value).HasColumnType("nvarchar(200)");
        builder
            .HasOne(rc => rc.Role).WithMany(r => r.RoleClaims).HasForeignKey(rc => rc.RoleId);
    }
}
using Domain.Entities;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // builder.IsMultiTenant();

        builder.HasKey(user => user.Id);
        builder
            .Property(user => user.Email)
            .HasColumnType("nvarchar(200)");
        builder
            .Property(user => user.Password)
            .HasColumnType("nvarchar(1000)");
    }
}
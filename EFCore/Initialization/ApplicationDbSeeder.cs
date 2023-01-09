using Domain.Entities;
using EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication.Interface;
using Shared.Authorization;

namespace EFCore.Initialization;

public class ApplicationDbSeeder
{
    private readonly VHNTenantInfo _currentTenant;
    private readonly IPasswordHelper _passwordHelper;

    public ApplicationDbSeeder(VHNTenantInfo currentTenant, IPasswordHelper passwordHelper)
    {
        _currentTenant = currentTenant;
        _passwordHelper = passwordHelper;
    }

    public async Task SeedDatabaseAsync(string adminPassword, AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await SeedRolesAsync(dbContext, cancellationToken);
        await SeedAdminUserAsync(adminPassword, dbContext, cancellationToken);
    }

    private async Task SeedRolesAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        foreach (var roleName in Roles.DefaultRoles)
        {
            var role = new Role
            {
                Name = roleName,
                Description = $"{roleName} Role for {_currentTenant.Id} Tenant",
            };
            await dbContext.Roles.AddAsync(role, cancellationToken);

            switch (roleName)
            {
                case Roles.Basic:
                    await AssignPermissionsToRoleAsync(dbContext, Permissions.Basic, role, cancellationToken);
                    break;
                case Roles.Admin:
                {
                    await AssignPermissionsToRoleAsync(dbContext, Permissions.Admin, role, cancellationToken);

                    break;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static async Task AssignPermissionsToRoleAsync(AppDbContext dbContext, IEnumerable<Permission> permissions,
        Role role, CancellationToken cancellationToken)
    {
        await dbContext.AddRangeAsync(permissions.Select(p => new RoleClaim
        {
            RoleId = role.Id,
            Value = p.Name
        }), cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedAdminUserAsync(string adminPassword, AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
        {
            return;
        }

        var adminUser = new User
        {
            FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
            LastName = Roles.Admin,
            Email = _currentTenant.AdminEmail,
            Password = _passwordHelper.HashPassword(adminPassword),
        };

        await dbContext.Users.AddAsync(adminUser, cancellationToken);

        // Assign role to user
        var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == Roles.Admin, cancellationToken);
        if (adminRole != null)
        {
            await dbContext.UserRoles.AddAsync(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            }, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
using Domain.Entities;
using EFCore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace API.Authentication.Permissions;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAppRepository<Role> _roleRepository;


    public PermissionAuthorizationHandler(IAppRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.GetUserId() is { } userId && await _hasPermissionAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }

    private Task<bool> _hasPermissionAsync(Guid? userId, string permission)
    {
        return _roleRepository
            .NoTrackingQuery(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .SelectMany(r => r.RoleClaims)
            .Select(rc => rc.Value)
            .ContainsAsync(permission);
    }
}
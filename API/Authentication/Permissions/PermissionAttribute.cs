using HotChocolate.AspNetCore.Authorization;
using Shared.Authorization;

namespace API.Authentication.Permissions;

public class PermissionAttribute : AuthorizeAttribute
{
    public PermissionAttribute(string action, string resource) =>
        Policy = Permission.NameFor(action, resource);
}
using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class Permissions
{
    private static readonly Permission[] _all = new Permission[]
    {
        new("View Users", Actions.View, Resources.Users, new List<string>() { Roles.Admin }),
        new("Search Users", Actions.Search, Resources.Users, new List<string>() { Roles.Admin }),
        new("Create Users", Actions.Create, Resources.Users, new List<string>() { Roles.Admin }),
        new("Update Users", Actions.Update, Resources.Users, new List<string>() { Roles.Admin }),
        new("Delete Users", Actions.Delete, Resources.Users, new List<string>() { Roles.Admin }),
        new("View UserRoles", Actions.View, Resources.UserRoles, new List<string>() { Roles.Admin }),
        new("Update UserRoles", Actions.Update, Resources.UserRoles, new List<string>() { Roles.Admin }),
        new("View Roles", Actions.View, Resources.Roles, new List<string>() { Roles.Admin }),
        new("Create Roles", Actions.Create, Resources.Roles, new List<string>() { Roles.Admin }),
        new("Update Roles", Actions.Update, Resources.Roles, new List<string>() { Roles.Admin }),
        new("Delete Roles", Actions.Delete, Resources.Roles, new List<string>() { Roles.Admin }),
        new("View RoleClaims", Actions.View, Resources.RoleClaims, new List<string>() { Roles.Admin }),
        new("Update RoleClaims", Actions.Update, Resources.RoleClaims, new List<string>() { Roles.Admin }),
        // new("View Tenants", Action.View, Resource.Tenants, IsRoot: true),
        // new("Create Tenants", Action.Create, Resource.Tenants, IsRoot: true),
        // new("Update Tenants", Action.Update, Resource.Tenants, IsRoot: true),
        // new("Upgrade Tenant Subscription", Action.UpgradeSubscription, Resource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);

    // public static IReadOnlyList<Permission> Root { get; } =
    //     new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot).ToArray());

    public static IReadOnlyList<Permission> Admin { get; } =
        new ReadOnlyCollection<Permission>(_all.Where(p => p.Roles.Contains(Roles.Admin)).ToList());

    public static IReadOnlyList<Permission> Basic { get; } =
        new ReadOnlyCollection<Permission>(_all.Where(p => p.Roles.Contains(Roles.Admin)).ToList());
}

public record Permission(string Description, string Action, string Resource, List<string> Roles)
{
    public string Name => _nameFor(Action, Resource);
    public static string NameFor(string action, string resource) => _nameFor(action, resource);

    private static string _nameFor(string action, string resource) => $"{nameof(Permissions)}.{resource}.{action}";
}
using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class Action
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);

    public const string Delete = nameof(Delete);
    // public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class Resource
{
    // public const string Tenants = nameof(Tenants);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
}

public static class Permissions
{
    private static readonly Permission[] _all = new Permission[]
    {
        new("View Users", Action.View, Resource.Users),
        new("Search Users", Action.Search, Resource.Users),
        new("Create Users", Action.Create, Resource.Users),
        new("Update Users", Action.Update, Resource.Users),
        new("Delete Users", Action.Delete, Resource.Users),
        new("View UserRoles", Action.View, Resource.UserRoles),
        new("Update UserRoles", Action.Update, Resource.UserRoles),
        new("View Roles", Action.View, Resource.Roles),
        new("Create Roles", Action.Create, Resource.Roles),
        new("Update Roles", Action.Update, Resource.Roles),
        new("Delete Roles", Action.Delete, Resource.Roles),
        new("View RoleClaims", Action.View, Resource.RoleClaims),
        new("Update RoleClaims", Action.Update, Resource.RoleClaims),
        // new("View Tenants", Action.View, Resource.Tenants, IsRoot: true),
        // new("Create Tenants", Action.Create, Resource.Tenants, IsRoot: true),
        // new("Update Tenants", Action.Update, Resource.Tenants, IsRoot: true),
        // new("Upgrade Tenant Subscription", Action.UpgradeSubscription, Resource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);

    // public static IReadOnlyList<Permission> Root { get; } =
    //     new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot).ToArray());

    public static IReadOnlyList<Permission> Admin { get; } =
        new ReadOnlyCollection<Permission>(_all.ToList());

    public static IReadOnlyList<Permission> Basic { get; } =
        new ReadOnlyCollection<Permission>(_all.Where(p => p.IsBasic).ToList());
}

public record Permission(string Description, string Action, string Resource, bool IsBasic = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
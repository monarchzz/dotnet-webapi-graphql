namespace Domain.Entities;

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ICollection<RoleClaim> RoleClaims { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = null!;
}
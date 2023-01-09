﻿namespace Domain.Entities;

public class RoleClaim
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public string Value { get; set; } = null!;
}
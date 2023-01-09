using Finbuckle.MultiTenant;

namespace Domain.Entities;

public class VHNTenantInfo : ITenantInfo
{
    public string Id { get; set; } = null!;

    // subdomain
    public string Identifier { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ConnectionString { get; set; } = null!;

    public string AdminEmail { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime ValidUpto { get; set; }

    public void SetValidity(in DateTime validTill) => ValidUpto = validTill;


    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    string? ITenantInfo.Id
    {
        get => Id;
        set => Id = value ?? throw new InvalidOperationException("Id can't be null.");
    }

    string? ITenantInfo.Identifier
    {
        get => Identifier;
        set => Identifier = value ?? throw new InvalidOperationException("Identifier can't be null.");
    }

    string? ITenantInfo.Name
    {
        get => Name;
        set => Name = value ?? throw new InvalidOperationException("Name can't be null.");
    }

    string? ITenantInfo.ConnectionString
    {
        get => ConnectionString;
        set => ConnectionString = value ?? throw new InvalidOperationException("ConnectionString can't be null.");
    }
}
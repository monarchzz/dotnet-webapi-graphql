namespace API.Resolvers.Tenants.Dtos;

public class CreateTenantInput
{
    public string Identifier { get; set; } = null!;

    public string TenantName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
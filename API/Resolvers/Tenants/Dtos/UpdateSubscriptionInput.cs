namespace API.Resolvers.Tenants.Dtos;

public class UpdateSubscriptionInput
{
    public string Id { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }
}
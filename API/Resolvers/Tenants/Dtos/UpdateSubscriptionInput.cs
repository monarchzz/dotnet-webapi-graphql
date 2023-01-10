namespace API.Resolvers.Tenants.Dtos;

public class UpdateSubscriptionInput
{
    public string Id { get; set; }

    public DateTime ExpiryDate { get; set; }
}
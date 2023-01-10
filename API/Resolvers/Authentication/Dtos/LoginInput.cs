namespace API.Resolvers.Authentication.Dtos;

public class LoginInput
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
namespace API.Resolvers.Authentication.Dtos;

public class ChangePasswordInput
{
    public string CurrentPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;
}
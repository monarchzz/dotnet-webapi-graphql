namespace EFCore.ConnectionString;

public interface IConnectionStringSecurer
{
    string? MakeSecure(string? connectionString);
}
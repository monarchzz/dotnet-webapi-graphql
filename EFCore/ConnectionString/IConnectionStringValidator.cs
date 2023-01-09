namespace EFCore.ConnectionString;

public interface IConnectionStringValidator
{
    bool TryValidate(string connectionString);
}
namespace EFCore.ConnectionString;

public interface IConnectionStringGenerator
{
    string Generate(string name);
}
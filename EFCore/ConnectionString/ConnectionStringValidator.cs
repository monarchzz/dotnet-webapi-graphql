using Microsoft.Data.SqlClient;

namespace EFCore.ConnectionString;

public class ConnectionStringValidator : IConnectionStringValidator
{
    public bool TryValidate(string connectionString)
    {
        try
        {
            var _ = new SqlConnectionStringBuilder(connectionString);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
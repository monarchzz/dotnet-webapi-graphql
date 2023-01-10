using Microsoft.Data.SqlClient;

namespace EFCore.ConnectionString;

public class ConnectionStringSecurer : IConnectionStringSecurer
{
    private const string HiddenValueDefault = "*******";

    public string? MakeSecure(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        return MakeSecureSqlConnectionString(connectionString);
    }

    private static string MakeSecureSqlConnectionString(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
        {
            builder.UserID = HiddenValueDefault;
        }

        return builder.ToString();
    }
}
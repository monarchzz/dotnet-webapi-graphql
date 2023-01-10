using EFCore.Common;
using Microsoft.Extensions.Options;

namespace EFCore.ConnectionString;

public class ConnectionStringGenerator : IConnectionStringGenerator
{
    private readonly IOptions<DatabaseSettings> _databaseSettings;

    public ConnectionStringGenerator(IOptions<DatabaseSettings> dbSettings)
    {
        _databaseSettings = dbSettings;
    }

    public string Generate(string name)
    {
        return _databaseSettings.Value.ConnectionStringTemplate.Replace("{DBName}", name);
    }
}
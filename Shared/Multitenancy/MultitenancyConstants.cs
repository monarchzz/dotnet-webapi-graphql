namespace Shared.Multitenancy;

public class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";
    }

    public const string DefaultPassword = "123456";

    public const string TenantIdName = "tenant";
}
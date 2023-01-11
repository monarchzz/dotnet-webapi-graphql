using API.Errors;

namespace API.Errors;

public static partial class AppErrors
{
    public static class Tenant
    {
        public static GraphqlError AlreadyExists =>
            new GraphqlError(code: "Tenant.AlreadyExists", message: "Tenant already exists.");

        public static GraphqlError NotExists =>
            new GraphqlError(code: "Tenant.NotExists", message: "Tenant does not exist.");

        public static GraphqlError TenantIsRequired =>
            new GraphqlError(code: "Tenant.TenantIsRequired", message: "Tenant is required.");

        public static GraphqlError EmailAlreadyExists =>
            new GraphqlError(code: "Tenant.EmailAlreadyExists", message: "Email already exists.");
    }
}
using Domain.Entities;
using Shared.Authorization;

namespace Shared.Multitenancy;

public static class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";

        public static User RootUser => new User()
        {
            FirstName = Name,
            LastName = Roles.Admin,
            Email = EmailAddress,
            Password = DefaultPassword,
        };
    }

    //123456
    public const string DefaultPassword = "$2a$11$Jh1tuMyl9FpxfrwufarFfekPS23t0yr/tBu9o2inViJNrBPqFS3h.";

    public const string TenantIdName = "tenant";
}
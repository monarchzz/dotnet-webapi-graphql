namespace API.Errors;

public static partial class AppErrors
{
    public static class Authentication
    {
        public static GraphqlError InvalidCredentials =>
            new GraphqlError(code: "Authentication.InvalidCredentials", message: "Invalid credentials");

        public static GraphqlError TokenExpiredOrInvalid =>
            new GraphqlError(code: "Authentication.TokenExpiredOrInvalid", message: "Token expired or invalid");

        public static GraphqlError EmailAlreadyExists =>
            new GraphqlError(code: "Authentication.EmailAlreadyExists", message: "Email already exists");
    }
}
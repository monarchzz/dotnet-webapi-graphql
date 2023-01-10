namespace API.Errors;

public class GraphqlError : GraphQLException
{
    public GraphqlError(string code, string message) : base(ErrorBuilder.New().SetCode(code).SetMessage(message).Build())
    {
    }
}
namespace AwesomeResultTests;

public sealed record TestError(int Code, string Message) : IError;

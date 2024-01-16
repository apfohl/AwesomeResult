namespace AwesomeResultTests;

public abstract record DomainError;

[GenerateResult<DomainError>]
public readonly partial struct Res<T>;

public static class GeneratedTests
{
    [Test]
    public static void Test()
    {
        Res<int> result = new();
    }
}

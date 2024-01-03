namespace AwesomeResultTests;

public static class TryExtensionsTest
{
    [Test]
    public static void Try_with_successful_function_returns_result_with_value()
    {
        var func = () => 42;

        func.Try().Match(value => value.Should().Be(42), _ => Assert.Fail());
    }

    [Test]
    public static void Try_with_throwing_function_returns_result_with_error()
    {
        var exception = new Exception("Test Exception");
        Func<int> func = () => throw exception;

        func.Try().Match(
            _ => Assert.Fail(),
            errors => errors
                .Should().ContainSingle(
                    error => error is DefaultExceptionError &&
                             error.Message.Equals(exception.Message) &&
                             error.Code.Equals(exception.GetHashCode())
                )
        );
    }

    [Test]
    public static void Try_with_throwing_function_executes_exception_handler()
    {
        Func<int> func = () => throw new Exception("Test Exception");

        func.Try(exception => new TestError(exception.HResult, exception.Message))
            .Match(
                _ => Assert.Fail(),
                errors => errors.Should().ContainSingle(error => error is TestError)
            );
    }
}

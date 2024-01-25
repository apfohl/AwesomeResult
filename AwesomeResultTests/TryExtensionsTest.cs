namespace AwesomeResultTests;

public static class TryExtensionsTest
{
    [Test]
    public static void Try_with_successful_function_returns_result_with_value()
    {
        var func = () => 42;

        func.Try().Switch(value => value.Should().Be(42), _ => Assert.Fail());
    }

    [Test]
    public static void Try_with_throwing_function_returns_result_with_default_exception_error()
    {
        var exception = new Exception("Test Exception");
        Func<int> func = () => throw exception;

        func.Try()
            .Switch(
                _ => Assert.Fail(),
                errors => errors.Should()
                    .ContainSingle(error => ((DefaultExceptionError)error).Exception.Equals(exception))
            );
    }

    [Test]
    public static void Try_with_null_exception_handler_throws_exception()
    {
        Func<int> func = () => throw new Exception("Test Exception");

        Assert.Throws<ArgumentNullException>(() => func.Try(null));
    }

    [Test]
    public static void Try_with_throwing_function_executes_exception_handler()
    {
        Func<int> func = () => throw new Exception("Test Exception");

        func.Try(exception => new TestError(exception.HResult, exception.Message))
            .Switch(
                _ => Assert.Fail(),
                errors => errors.Should().ContainSingle(error => error != null)
            );
    }
}

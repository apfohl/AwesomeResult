namespace AwesomeResultTests;

public static class TryAsyncExtensionsTests
{
    [Test]
    public static async Task TryAsync_with_successful_async_function_returns_result_task_with_result()
    {
        var func = () => Task.FromResult(42);

        var result = await func.TryAsync();

        result.Match(value => value, _ => 23).Should().Be(42);
    }

    [Test]
    public static async Task TryAsync_with_throwing_function_returns_result_with_default_exception_error()
    {
        var exception = new Exception("Test Exception");
        Func<Task<int>> func = () => throw exception;

        var result = await func.TryAsync();

        result.Match(
            _ => new List<IError>(),
            errors => errors
        ).Should().ContainSingle(error => error is DefaultExceptionError);
    }

    [Test]
    public static async Task TryAsync_with_null_exception_handler_throws_exception()
    {
        var func = () => Task.FromResult(42);

        var result = async () => await func.TryAsync((Func<Exception, IError>)null);

        await result.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public static async Task
        TryAsync_with_successful_async_function_and_custom_exception_handler_returns_task_with_result()
    {
        var func = () => Task.FromResult(42);

        var result = await func.TryAsync(e => new TestError(e.GetHashCode(), e.Message));

        result.Match(value => value, _ => 23).Should().Be(42);
    }

    [Test]
    public static async Task
        TryAsync_with_throwing_function_and_custom_exception_handler_returns_result_with_custom_exception_error()
    {
        var exception = new Exception("Test Exception");
        Func<Task<int>> func = () => throw exception;

        var result = await func.TryAsync(e => new TestError(e.GetHashCode(), exception.Message));

        result.Match(
            _ => new List<IError>(),
            errors => errors
        ).Should().ContainSingle(error => error is TestError);
    }

    [Test]
    public static async Task TryAsync_with_null_async_exception_handler_throws_exception()
    {
        var func = () => Task.FromResult(42);

        var result = async () => await func.TryAsync((Func<Exception, Task<IError>>)null);

        await result.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public static async Task
        TryAsync_with_successful_async_function_and_custom_async_exception_handler_returns_task_with_result()
    {
        var func = () => Task.FromResult(42);

        var result = await func.TryAsync(e => Task.FromResult<IError>(new TestError(e.GetHashCode(), e.Message)));

        result.Match(value => value, _ => 23).Should().Be(42);
    }

    [Test]
    public static async Task
        TryAsync_with_throwing_function_and_custom_async_exception_handler_returns_result_with_custom_exception_error()
    {
        var exception = new Exception("Test Exception");
        Func<Task<int>> func = () => throw exception;

        var result = await func.TryAsync(e => Task.FromResult<IError>(new TestError(e.GetHashCode(), e.Message)));

        result.Match(
            _ => new List<IError>(),
            errors => errors
        ).Should().ContainSingle(error => error is TestError);
    }
}

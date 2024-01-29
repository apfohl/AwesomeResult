namespace AwesomeResultTests;

public static class AsyncExtensionsTests
{
    [Test]
    public static async Task SelectAsync_success_result_with_async_mapping_returns_success_result_task()
    {
        Result<int> result = 42;

        await result.SelectAsync(value => Task.FromResult($"{value}"))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task
        SelectAsync_failure_result_with_async_mapping_returns_failure_result_task_with_same_errors()
    {
        var error = new TestError(42, string.Empty);
        var result = error.Fail<int>();

        await result.SelectAsync(value => Task.FromResult($"{value}"))
            .SwitchAsync(
                _ => Assert.Fail(),
                errors => errors.Should().ContainSingle(e => e.Equals(error))
            );
    }

    [Test]
    public static async Task SelectAsync_success_result_task_with_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.SelectAsync(value => $"{value}")
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task SelectAsync_success_result_task_with_async_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.SelectAsync(value => Task.FromResult($"{value}"))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task MapAsync_success_result_with_async_mapping_returns_success_result_task()
    {
        Result<int> result = 42;

        await result.MapAsync(value => Task.FromResult($"{value}"))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task MapAsync_success_result_Task_with_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.MapAsync(value => $"{value}")
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task MapAsync_success_result_task_with_async_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.MapAsync(value => Task.FromResult($"{value}"))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task SelectManyAsync_success_result_with_async_mapping_returns_success_result_task()
    {
        Result<int> result = 42;

        await result.SelectManyAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task
        SelectManyAsync_failure_result_with_async_mapping_returns_failure_result_task_with_same_errors()
    {
        var error = new TestError(42, string.Empty);
        var result = error.Fail<int>();

        await result.SelectManyAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                _ => Assert.Fail(),
                errors => errors.Should().ContainSingle(e => e.Equals(error))
            );
    }

    [Test]
    public static async Task SelectManyAsync_success_result_task_with_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.SelectManyAsync(value => $"{value}".Success())
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task SelectManyAsync_success_result_task_with_async_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.SelectManyAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task BindAsync_success_result_with_async_mapping_returns_success_result_task()
    {
        Result<int> result = 42;

        await result.BindAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task BindAsync_success_result_task_with_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.BindAsync(value => $"{value}".Success())
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task BindAsync_success_result_task_with_async_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.BindAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task FlatMapAsync_success_result_with_async_mapping_returns_success_result_task()
    {
        Result<int> result = 42;

        await result.FlatMapAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task FlatMapAsync_success_result_task_with_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.FlatMapAsync(value => $"{value}".Success())
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task FlatMapAsync_success_result_task_with_async_mapping_returns_success_result_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        await result.FlatMapAsync(value => Task.FromResult($"{value}".Success()))
            .SwitchAsync(
                value => value.Should().Be("42"),
                _ => Assert.Fail()
            );
    }

    [Test]
    public static async Task MatchAsync_success_result_with_async_handlers_returns_success_value_task()
    {
        Result<int> result = 42;

        (await result.MatchAsync(
            Task.FromResult,
            _ => Task.FromResult(23)
        )).Should().Be(42);
    }

    [Test]
    public static async Task MatchAsync_failure_result_with_async_handlers_returns_failure_value_task()
    {
        var error = new TestError(42, string.Empty);
        var result = error.Fail<int>();

        (await result.MatchAsync(
            Task.FromResult,
            _ => Task.FromResult(23)
        )).Should().Be(23);
    }

    [Test]
    public static async Task MatchAsync_success_result_task_with_handlers_returns_success_value_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        (await result.MatchAsync(
            value => value,
            _ => 23
        )).Should().Be(42);
    }

    [Test]
    public static async Task MatchAsync_success_result_task_with_async_handlers_returns_success_value_task()
    {
        var result = Task.FromResult<Result<int>>(42);

        (await result.MatchAsync(
            Task.FromResult,
            _ => Task.FromResult(23)
        )).Should().Be(42);
    }

    [Test]
    public static async Task MatchAsync_failure_result_task_with_async_handlers_returns_failure_value_task()
    {
        var error = new TestError(42, string.Empty);
        var result = Task.FromResult(error.Fail<int>());

        (await result.MatchAsync(
            Task.FromResult,
            _ => Task.FromResult(23)
        )).Should().Be(23);
    }
}

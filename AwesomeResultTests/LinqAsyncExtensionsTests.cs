namespace AwesomeResultTests;

public static class LinqAsyncExtensionsTests
{
    [Test]
    public static async Task Select_from_result_task_with_value_returns_result_with_value()
    {
        const string input = "Test";
        var result = Task.FromResult<Result<string>>(input);

        (await (from s in result select s).MatchAsync(value => value, _ => "Foo")).Should().Be(input);
    }

    [Test]
    public static void SelectMany_from_result_task_with_async_null_collection_throws_exception() =>
        Assert.Throws<ArgumentNullException>(
            () => Task.FromResult("Test".Success()).SelectMany(
                ((Func<string, Task<Result<string>>>)null)!,
                (i, c) => $"{i}{c}"
            )
        );

    [Test]
    public static void SelectMany_with_null_selector_throws_exception() =>
        Assert.Throws<ArgumentNullException>(
            () => Task.FromResult("Test".Success()).SelectMany(
                s => Task.FromResult(s.Success()),
                ((Func<string, string, string>)null)!)
        );

    [Test]
    public static async Task SelectMany_from_result_task_with_value_returns_result_task_with_value()
    {
        const int input = 42;
        var result1 = Task.FromResult<Result<int>>(input);
        var result2 = Task.FromResult<Result<int>>(input);

        (await (
            from s in result1
            from i in result2
            select i
        )).Should().Be(input.Success());
    }
}

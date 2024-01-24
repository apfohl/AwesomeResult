namespace AwesomeResultTests;

public static class LinqExtensionsTest
{
    [Test]
    public static void Select_from_result_with_value_returns_result_with_value()
    {
        const string input = "Test";
        (from s in input.Success<string, TestError>() select s)
            .Should()
            .Be(input.Success<string, TestError>());
    }

    [Test]
    public static void SelectMany_with_null_collection_throws_exception() =>
        Assert.Throws<ArgumentNullException>(
            () => "Test".Success<string, TestError>().SelectMany(
                ((Func<string, Result<string, TestError>>)null)!,
                (i, c) => $"{i}{c}"
            ).Should()
        );

    [Test]
    public static void SelectMany_with_null_selector_throws_exception() =>
        Assert.Throws<ArgumentNullException>(
            () => "Test".Success<string, TestError>().SelectMany(
                s => s.Success<string, TestError>(),
                ((Func<string, string, string>)null)!)
        );

    [Test]
    public static void SelectMany_from_result_with_value_returns_result_with_value()
    {
        const int input = 42;

        (
            from s in "Test".Success<string, TestError>()
            from i in input.Success<int, TestError>()
            select i
        ).Should().Be(input.Success<int, TestError>());
    }
}

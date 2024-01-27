namespace AwesomeResultTests;

public static class LinqExtensionsTests
{
    [Test]
    public static void Select_from_result_with_value_returns_result_with_value()
    {
        const string input = "Test";
        (from s in input.Success() select s)
            .Should()
            .Be(input.Success());
    }

    [Test]
    public static void SelectMany_with_null_collection_throws_exception() =>
        Assert.Throws<ArgumentNullException>(
            () => "Test".Success().SelectMany(
                ((Func<string, Result<string>>)null)!,
                (i, c) => $"{i}{c}"
            ).Should()
        );

    [Test]
    public static void SelectMany_with_null_selector_throws_exception() =>
        Assert.Throws<ArgumentNullException>(
            () => "Test".Success().SelectMany(
                s => s.Success(),
                ((Func<string, string, string>)null)!)
        );

    [Test]
    public static void SelectMany_from_result_with_value_returns_result_with_value()
    {
        const int input = 42;

        (
            from s in "Test".Success()
            from i in input.Success()
            select i
        ).Should().Be(input.Success());
    }
}

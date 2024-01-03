using AwesomeResult;

namespace AwesomeResultTests;

public static class ResultTests
{
    private sealed record TestError(int Code, string Message) : IError;

    [Test]
    public static void Equals_with_same_values_is_true()
    {
        Result<int> left = 42;
        Result<int> right = 42;

        left.Should().Be(right);
    }

    [Test]
    public static void Equals_with_different_types_is_false()
    {
        Result<string> left = "Hello";
        Result<int> right = 42;

        left.Should().NotBe(right);
    }

    [Test]
    public static void Equals_with_different_values_is_false()
    {
        Result<int> left = 42;
        Result<int> right = 23;

        left.Should().NotBe(right);
    }

    [Test]
    public static void Equals_with_same_errors_is_true()
    {
        var left = new[]
        {
            new TestError(42, "Not the truth!"),
            new TestError(23, "Nothing is like it seems!")
        }.Fail<int>();

        var right = new[]
        {
            new TestError(42, "Not the truth!"),
            new TestError(23, "Nothing is like it seems!")
        }.Fail<int>();

        left.Should().Be(right);
    }

    [Test]
    public static void Equals_with_different_number_errors_is_false()
    {
        var left = new[]
        {
            new TestError(42, "Not the truth!"), new TestError(23, "Nothing is like it seems!")
        }.Fail<int>();

        var right = new[]
        {
            new TestError(42, "Not the truth!")
        }.Fail<int>();

        left.Should().NotBe(right);
    }

    [Test]
    public static void GetHashCode_with_same_values_is_equal()
    {
        Result<int> left = 42;
        Result<int> right = 42;

        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    [Test]
    public static void GetHashCode_with_different_values_is_not_equal()
    {
        Result<int> left = 42;
        Result<int> right = 23;

        left.GetHashCode().Should().NotBe(right.GetHashCode());
    }

    [Test]
    public static void GetHashCode_with_same_errors_is_equal()
    {
        var left = new[]
        {
            new TestError(42, "Not the truth!"),
            new TestError(23, "Nothing is like it seems!")
        }.Fail<int>();

        var right = new[]
        {
            new TestError(42, "Not the truth!"),
            new TestError(23, "Nothing is like it seems!")
        }.Fail<int>();

        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    [Test]
    public static void GetHashCode_with_different_errors_is_not_equal()
    {
        var left = new[]
        {
            new TestError(42, "Not the truth!")
        }.Fail<int>();

        var right = new[]
        {
            new TestError(23, "Nothing is like it seems!")
        }.Fail<int>();

        left.GetHashCode().Should().NotBe(right.GetHashCode());
    }
}

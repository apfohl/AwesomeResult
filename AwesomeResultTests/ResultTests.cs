using AwesomeResult.Initializers;

namespace AwesomeResultTests;

using static Initializer;

public static class ResultTests
{
    [Test]
    public static void Failure_create_result_with_empty_errors()
    {
        Test().Switch(_ => Assert.Fail(), errors => errors.Should().BeEmpty());
        return;

        Result<int> Test() => Failure;
    }

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

    [Test]
    public static void Match_with_null_success_action_throws_exception()
    {
        var test = () => 42.Success().Switch(null, _ => { });
        test.Should().Throw<ArgumentException>();
    }

    [Test]
    public static void Match_with_null_failure_action_throws_exception()
    {
        var test = () => 42.Success().Switch(_ => { }, null);
        test.Should().Throw<ArgumentException>();
    }

    [Test]
    public static void Match_with_null_success_function_throws_exception()
    {
        var test = () => 42.Success().Match(null, _ => 23);
        test.Should().Throw<ArgumentException>();
    }

    [Test]
    public static void Match_with_null_failure_function_throws_exception()
    {
        var test = () => 42.Success().Match(value => value, null);
        test.Should().Throw<ArgumentException>();
    }

    [Test]
    public static void Match_with_successful_result_executes_success_handler() =>
        42.Success().Match(value => value, _ => default).Should().Be(42);

    [Test]
    public static void Match_with_failed_result_executes_failure_handler() =>
        new TestError(42, "Not the truth!").Fail<int>().Match(_ => default, _ => 42).Should().Be(42);

    [Test]
    public static void Select_with_null_selector_throws_Exception()
    {
        var test = () => 42.Success().Select<int>(null);
        test.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public static void Select_with_success_result_maps_to_new_value() =>
        42.Success().Select(value => value.ToString())
            .Switch(result => result.Should().Be("42"), _ => Assert.Fail());

    [Test]
    public static void Select_with_failed_result_keeps_errors()
    {
        var error = new TestError(42, "Not the truth!");

        error.Fail<int>().Select(value => value.ToString()).Switch(
            _ => Assert.Fail(),
            errors => errors.Should().ContainSingle(e => e.Equals(error))
        );
    }

    [Test]
    public static void SelectMany_with_null_selector_throws_Exception()
    {
        var test = () => 42.Success().SelectMany<int>(null);
        test.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public static void SelectMany_with_success_result_maps_to_new_value() =>
        42.Success().SelectMany(value => value.ToString().Success())
            .Switch(
                result => result.Should().Be("42"),
                _ => Assert.Fail()
            );

    [Test]
    public static void SelectMany_with_failed_result_keeps_errors()
    {
        var error = new TestError(42, "Not the truth!");

        error.Fail<int>().SelectMany(value => value.ToString().Success()).Switch(
            _ => Assert.Fail(),
            errors => errors.Should().ContainSingle(e => e.Equals(error))
        );
    }

    [Test]
    public static void GetHashCode_of_failure_struct_is_zero() =>
        new Failure().GetHashCode().Should().Be(0);

    [Test]
    public static void Equals_of_failure_and_failure_is_true() =>
        new Failure().Should().Be(new Failure());

    [Test]
    public static void Equals_of_failure_and_int_if_false() =>
        new Failure().Should().NotBe(42);
}

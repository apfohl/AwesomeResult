namespace AwesomeResultTests;

using static Initializer;

public static class ResultExtensionsTests
{
    [Test]
    public static void Success_creates_result_with_value() =>
        42.Success().Switch(_ => Assert.Pass(), _ => Assert.Fail());

    [Test]
    public static void Fail_with_error_creates_result_with_error() =>
        new TestError(42, "Not the truth!")
            .Fail<int>()
            .Switch(_ => Assert.Fail(), _ => Assert.Pass());

    [Test]
    public static void Fail_with_errors_creates_result_with_errors() =>
        new List<TestError>
            {
                new(42, "Not the truth!"),
                new(43, "Another error")
            }
            .Fail<int>()
            .Switch(_ => Assert.Fail(), _ => Assert.Pass());

    [Test]
    public static void Map_with_null_selector_throws_Exception()
    {
        var test = () => 42.Success().Map((Func<int, string>)null);
        test.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public static void Map_with_success_result_maps_to_new_value() =>
        42.Success().Map(value => value.ToString())
            .Switch(result => result.Should().Be("42"), _ => Assert.Fail());

    [Test]
    public static void Map_with_failed_result_keeps_errors()
    {
        var error = new TestError(42, "Not the truth!");

        error.Fail<int>().Map(value => value.ToString()).Switch(
            _ => Assert.Fail(),
            errors => errors.Should().ContainSingle(e => e.Equals(error))
        );
    }

    [Test]
    public static void FlatMap_with_null_selector_throws_Exception()
    {
        var test = () => 42.Success().FlatMap<int, string>(null);
        test.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public static void FlatMap_with_success_result_maps_to_new_value() =>
        42.Success().FlatMap(value => value.ToString().Success()).Switch(
            result => result.Should().Be("42"),
            _ => Assert.Fail()
        );

    [Test]
    public static void FlatMap_with_failed_result_keeps_errors()
    {
        var error = new TestError(42, "Not the truth!");

        error.Fail<int>().FlatMap(value => value.ToString().Success()).Switch(
            _ => Assert.Fail(),
            errors => errors.Should().ContainSingle(e => e.Equals(error))
        );
    }

    [Test]
    public static void Bind_with_null_selector_throws_Exception()
    {
        var test = () => 42.Success().Bind<int, string>(null);
        test.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public static void Bind_with_success_result_maps_to_new_value() =>
        42.Success().Bind(value => value.ToString().Success()).Switch(
            result => result.Should().Be("42"),
            _ => Assert.Fail()
        );

    [Test]
    public static void Bind_with_failed_result_keeps_errors()
    {
        var error = new TestError(42, "Not the truth!");

        error.Fail<int>().Bind(value => value.ToString().Success()).Switch(
            _ => Assert.Fail(),
            errors => errors.Should().ContainSingle(e => e.Equals(error))
        );
    }

    [Test]
    public static void OrElse_with_successful_result_returns_result_value() =>
        42.Success().OrElse(23).Should().Be(42);

    [Test]
    public static void OrElse_with_failed_result_returns_or_value() =>
        ((Result<int>)Failure).OrElse(23).Should().Be(23);

    [Test]
    public static void OrElse_with_successful_result_and_result_function_returns_result_value() =>
        42.Success().OrElse(() => 23).Should().Be(42);

    [Test]
    public static void OrElse_with_failed_result_and_result_function_returns_or_value() =>
        ((Result<int>)Failure).OrElse(() => 23).Should().Be(23);

    [Test]
    public static void OrElse_with_successful_result_and_result_mapping_function_returns_result_value() =>
        42.Success().OrElse(_ => 23).Should().Be(42);

    [Test]
    public static void OrElse_with_failed_result_and_result_mapping_function_returns_or_value() =>
        ((Result<int>)Failure).OrElse(_ => 23).Should().Be(23);
}

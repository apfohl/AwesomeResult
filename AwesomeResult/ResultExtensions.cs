using System;
using System.Collections.Generic;

namespace AwesomeResult
{
    public static class ResultExtensions
    {
        public static Result<T, TFailure> Success<T, TFailure>(this T value) => value;

        public static Result<T, TFailure> Fail<T, TFailure>(this TFailure error) => Result<T, TFailure>.Of(error);

        public static Result<T, TFailure> Fail<T, TFailure>(this IEnumerable<TFailure> errors) =>
            Result<T, TFailure>.Of(errors);

        public static T OrElse<T, TFailure>(this Result<T, TFailure> result, T orElse) =>
            result.Match(value => value, _ => orElse);

        public static T OrElse<T, TFailure>(this Result<T, TFailure> result, Func<T> orElse) =>
            result.Match(value => value, _ => orElse());

        public static T OrElse<T, TFailure>(this Result<T, TFailure> result, Func<IReadOnlyList<TFailure>, T> orElse) =>
            result.Match(value => value, orElse);

        public static Result<TResult, TFailure> Map<T, TResult, TFailure>(this Result<T, TFailure> result,
            Func<T, TResult> mapping) => result.Select(mapping);

        public static Result<TResult, TFailure> FlatMap<T, TResult, TFailure>(this Result<T, TFailure> result,
            Func<T, Result<TResult, TFailure>> mapping) => result.SelectMany(mapping);

        public static Result<TResult, TFailure> Bind<T, TResult, TFailure>(this Result<T, TFailure> result,
            Func<T, Result<TResult, TFailure>> mapping) => result.SelectMany(mapping);
    }
}

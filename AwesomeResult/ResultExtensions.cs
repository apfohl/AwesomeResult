using System;
using System.Collections.Generic;

namespace AwesomeResult
{
    public static class ResultExtensions
    {
        public static Result<T> Success<T>(this T value) => value;

        public static Result<T> Fail<T>(this IError error) => Result<T>.Of(error);

        public static Result<T> Fail<T>(this IEnumerable<IError> errors) => Result<T>.Of(errors);

        public static T OrElse<T>(this Result<T> result, T orElse) =>
            result.Match(value => value, _ => orElse);

        public static T OrElse<T>(this Result<T> result, Func<T> orElse) =>
            result.Match(value => value, _ => orElse());

        public static T OrElse<T>(this Result<T> result, Func<IReadOnlyList<IError>, T> orElse) =>
            result.Match(value => value, orElse);

        public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, TResult> mapping) =>
            result.Select(mapping);

        public static Result<TResult> FlatMap<T, TResult>(this Result<T> result, Func<T, Result<TResult>> mapping) =>
            result.SelectMany(mapping);

        public static Result<TResult> Bind<T, TResult>(this Result<T> result, Func<T, Result<TResult>> mapping) =>
            result.SelectMany(mapping);
    }
}

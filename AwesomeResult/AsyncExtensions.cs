using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeResult
{
    public static class AsyncExtensions
    {
        public static Task<Result<TResult>> Select<T, TResult>(this Result<T> result, Func<T, Task<TResult>> mapping,
            bool continueOnCapturedContext = false) =>
            result.Select(mapping).Match(
                async task => (await task.ConfigureAwait(continueOnCapturedContext)).Success(),
                errors => Task.FromResult(errors.Fail<TResult>())
            );

        public static async Task<Result<TResult>> Select<T, TResult>(this Task<Result<T>> result,
            Func<T, TResult> mapping, bool continueOnCapturedContext = false) =>
            (await result.ConfigureAwait(continueOnCapturedContext)).Select(mapping);

        public static async Task<Result<TResult>> Select<T, TResult>(this Task<Result<T>> result,
            Func<T, Task<TResult>> mapping, bool continueOnCapturedContext = false) =>
            await (await result.ConfigureAwait(continueOnCapturedContext)).Select(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> Map<T, TResult>(this Result<T> result, Func<T, Task<TResult>> mapping,
            bool continueOnCapturedContext = false) =>
            result.Select(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> Map<T, TResult>(this Task<Result<T>> result,
            Func<T, TResult> mapping, bool continueOnCapturedContext = false) =>
            result.Select(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> Map<T, TResult>(this Task<Result<T>> result,
            Func<T, Task<TResult>> mapping, bool continueOnCapturedContext = false) =>
            result.Select(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> SelectMany<T, TResult>(this Result<T> result,
            Func<T, Task<Result<TResult>>> mapping, bool continueOnCapturedContext = false)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            return result.Match(
                async value => await mapping(value).ConfigureAwait(continueOnCapturedContext),
                errors => Task.FromResult(errors.Fail<TResult>()));
        }

        public static async Task<Result<TResult>> SelectMany<T, TResult>(this Task<Result<T>> result,
            Func<T, Result<TResult>> mapping, bool continueOnCapturedContext = false) =>
            (await result.ConfigureAwait(continueOnCapturedContext)).SelectMany(mapping);

        public static async Task<Result<TResult>> SelectMany<T, TResult>(this Task<Result<T>> result,
            Func<T, Task<Result<TResult>>> mapping, bool continueOnCapturedContext = false) =>
            await (await result.ConfigureAwait(continueOnCapturedContext)).SelectMany(mapping,
                continueOnCapturedContext);

        public static Task<Result<TResult>> Bind<T, TResult>(this Result<T> result,
            Func<T, Task<Result<TResult>>> mapping, bool continueOnCapturedContext = false) =>
            result.SelectMany(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> Bind<T, TResult>(this Task<Result<T>> result,
            Func<T, Result<TResult>> mapping, bool continueOnCapturedContext = false) =>
            result.SelectMany(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> Bind<T, TResult>(this Task<Result<T>> result,
            Func<T, Task<Result<TResult>>> mapping, bool continueOnCapturedContext = false) =>
            result.SelectMany(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> FlatMap<T, TResult>(this Result<T> result,
            Func<T, Task<Result<TResult>>> mapping, bool continueOnCapturedContext = false) =>
            result.SelectMany(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> FlatMap<T, TResult>(this Task<Result<T>> result,
            Func<T, Result<TResult>> mapping, bool continueOnCapturedContext = false) =>
            result.SelectMany(mapping, continueOnCapturedContext);

        public static Task<Result<TResult>> FlatMap<T, TResult>(this Task<Result<T>> result,
            Func<T, Task<Result<TResult>>> mapping, bool continueOnCapturedContext = false) =>
            result.SelectMany(mapping, continueOnCapturedContext);

        public static async Task<TResult> Match<T, TResult>(this Task<Result<T>> result,
            Func<T, TResult> success, Func<IReadOnlyList<IError>, TResult> failure,
            bool continueOnCapturedContext = false) =>
            (await result.ConfigureAwait(continueOnCapturedContext)).Match(success, failure);

        public static async Task<TResult> Match<T, TResult>(this Task<Result<T>> result,
            Func<T, Task<TResult>> success, Func<IReadOnlyList<IError>, Task<TResult>> failure,
            bool continueOnCapturedContext = false) =>
            await (await result).Match(
                async value => await success(value).ConfigureAwait(continueOnCapturedContext),
                async errors => await failure(errors).ConfigureAwait(continueOnCapturedContext)
            );

        public static Task<T> OrElse<T>(this Task<Result<T>> result, T orElse,
            bool continueOnCapturedContext = false) =>
            result.Match(value => value, _ => orElse, continueOnCapturedContext);

        public static Task<T> OrElse<T>(this Task<Result<T>> result, Task<T> orElse,
            bool continueOnCapturedContext = false) =>
            result.Match(
                Task.FromResult,
                async _ => await orElse.ConfigureAwait(continueOnCapturedContext),
                continueOnCapturedContext
            );

        public static Task<T> OrElse<T>(this Task<Result<T>> result, Func<IReadOnlyList<IError>, T> orElse,
            bool continueOnCapturedContext = false) =>
            result.Match(value => value, orElse, continueOnCapturedContext);

        public static Task<T> OrElse<T>(this Task<Result<T>> result, Func<IReadOnlyList<IError>, Task<T>> orElse,
            bool continueOnCapturedContext = false) =>
            result.Match(
                Task.FromResult,
                async errors => await orElse(errors).ConfigureAwait(continueOnCapturedContext),
                continueOnCapturedContext
            );
    }
}

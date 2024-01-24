using System;

namespace AwesomeResult
{
    public static class LinqExtensions
    {
        public static Result<TResult, TFailure> SelectMany<T, TCollection, TResult, TFailure>(
            this Result<T, TFailure> result,
            Func<T, Result<TCollection, TFailure>> collection,
            Func<T, TCollection, TResult> selector
        )
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return result.SelectMany(r => collection(r).Select(c => selector(r, c)));
        }
    }
}

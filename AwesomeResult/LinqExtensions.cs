using System;

namespace AwesomeResult
{
    public static class LinqExtensions
    {
        public static Result<TResult> SelectMany<T, TCollection, TResult>(
            this Result<T> result,
            Func<T, Result<TCollection>> collection,
            Func<T, TCollection, TResult> selector
        )
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return result.SelectMany(r => collection(r).Select(c => selector(r, c)));
        }
    }
}

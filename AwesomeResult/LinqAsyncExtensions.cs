using System;
using System.Threading.Tasks;

namespace AwesomeResult
{
    public static class LinqAsyncExtensions
    {
        public static Task<Result<TResult>> Select<T, TResult>(this Task<Result<T>> result, Func<T, TResult> mapping) =>
            result.SelectAsync(mapping);

        public static Task<Result<TResult>> SelectMany<T, TCollection, TResult>(
            this Task<Result<T>> result,
            Func<T, Task<Result<TCollection>>> collectionTask,
            Func<T, TCollection, TResult> selector)
        {
            if (collectionTask == null) throw new ArgumentNullException(nameof(collectionTask));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return result.SelectManyAsync(m => collectionTask(m).SelectAsync(value => selector(m, value)));
        }
    }
}

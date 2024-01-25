using System;
using System.Threading.Tasks;

namespace AwesomeResult
{
    public static class LinqAsyncExtensions
    {
        public static Task<Result<TResult>> SelectMany<T, TCollection, TResult>(
            this Task<Result<T>> result,
            Func<T, Task<Result<TCollection>>> collectionTask,
            Func<T, TCollection, TResult> selector)
        {
            if (collectionTask == null) throw new ArgumentNullException(nameof(collectionTask));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return result.SelectMany(m => collectionTask(m).Select(value => selector(m, value)));
        }
    }
}

using System;
using System.Threading.Tasks;

namespace AwesomeResult
{
    public static class TryAsyncExtensions
    {
        public static async Task<Result<T>> TryAsync<T>(this Func<Task<T>> func, bool continueOnCapturedContext = false)
        {
            try
            {
                return (await func().ConfigureAwait(continueOnCapturedContext)).Success();
            }
            catch (Exception exception)
            {
                return DefaultExceptionError.Create(exception).Fail<T>();
            }
        }

        public static async Task<Result<T>> TryAsync<T>(this Func<Task<T>> func,
            Func<Exception, IError> exceptionHandler, bool continueOnCapturedContext = false)
        {
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            try
            {
                return (await func().ConfigureAwait(continueOnCapturedContext)).Success();
            }
            catch (Exception exception)
            {
                return exceptionHandler(exception).Fail<T>();
            }
        }

        public static async Task<Result<T>> TryAsync<T>(this Func<Task<T>> func,
            Func<Exception, Task<IError>> exceptionHandler, bool continueOnCapturedContext = false)
        {
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            try
            {
                return (await func().ConfigureAwait(continueOnCapturedContext)).Success();
            }
            catch (Exception exception)
            {
                return (await exceptionHandler(exception).ConfigureAwait(continueOnCapturedContext)).Fail<T>();
            }
        }
    }
}

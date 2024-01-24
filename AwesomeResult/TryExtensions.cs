using System;

namespace AwesomeResult
{
    public readonly struct DefaultExceptionError
    {
        public Exception Exception { get; }

        private DefaultExceptionError(Exception exception) =>
            Exception = exception;

        public static DefaultExceptionError Of(Exception exception) =>
            new DefaultExceptionError(exception);
    }

    public static class TryExtensions
    {
        public static Result<T, DefaultExceptionError> Try<T>(this Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                return DefaultExceptionError.Of(exception).Fail<T, DefaultExceptionError>();
            }
        }

        public static Result<T, TFailure> Try<T, TFailure>(this Func<T> func,
            Func<Exception, TFailure> exceptionHandler)
        {
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            try
            {
                return func();
            }
            catch (Exception exception)
            {
                return exceptionHandler(exception).Fail<T, TFailure>();
            }
        }
    }
}

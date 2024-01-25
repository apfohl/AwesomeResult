using System;

namespace AwesomeResult
{
    public readonly struct DefaultExceptionError : IError
    {
        public Exception Exception { get; }

        private DefaultExceptionError(Exception exception) => Exception = exception;

        public static DefaultExceptionError Create(Exception exception) => new DefaultExceptionError(exception);
    }

    public static class TryExtensions
    {
        public static Result<T> Try<T>(this Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                return DefaultExceptionError.Create(exception).Fail<T>();
            }
        }

        public static Result<T> Try<T>(this Func<T> func, Func<Exception, IError> exceptionHandler)
        {
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            try
            {
                return func();
            }
            catch (Exception exception)
            {
                return exceptionHandler(exception).Fail<T>();
            }
        }
    }
}

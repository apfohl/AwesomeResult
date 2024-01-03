using System;

namespace AwesomeResult
{
    public readonly struct DefaultExceptionError : IError
    {
        public int Code { get; }
        public string Message { get; }

        private DefaultExceptionError(Exception exception)
        {
            Code = exception.GetHashCode();
            Message = exception.Message;
        }

        public static DefaultExceptionError Of(Exception exception) =>
            new DefaultExceptionError(exception);
    }

    public static class TryExtensions
    {
        private static readonly Func<Exception, IError> DefaultExceptionHandler =
            exception => DefaultExceptionError.Of(exception);

        public static Result<T> Try<T>(this Func<T> func, Func<Exception, IError> exceptionHandler = null)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                return exceptionHandler switch
                {
                    null => DefaultExceptionHandler(exception).Fail<T>(),
                    _ => exceptionHandler(exception).Fail<T>()
                };
            }
        }
    }
}

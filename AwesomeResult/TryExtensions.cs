namespace AwesomeResult
{
    public readonly struct DefaultExceptionError : IError
    {
        public Exception Exception { get; }

        private DefaultExceptionError(Exception exception) => Exception = exception;

        public static DefaultExceptionError Of(Exception exception) => new(exception);
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
                return DefaultExceptionError.Of(exception).Fail<T>();
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

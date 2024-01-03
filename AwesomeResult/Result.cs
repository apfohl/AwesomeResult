using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeResult.SuccessInitializer;

namespace AwesomeResult
{
    public static class Initializer
    {
        public static Success Success = default;
    }

    public readonly struct Result<T> : IEquatable<Result<T>> where T : notnull
    {
        private T Instance { get; }

        private List<IError> Errors { get; }

        private bool IsSuccessful { get; }

        private Result(T instance)
        {
            Instance = instance;
            Errors = new List<IError>();
            IsSuccessful = true;
        }

        private Result(IError error)
        {
            Instance = default;
            Errors = new List<IError> { error };
            IsSuccessful = false;
        }

        private Result(IEnumerable<IError> errors)
        {
            Instance = default;
            Errors = errors.ToList();
            IsSuccessful = false;
        }

        public static implicit operator Result<T>(T value) => new Result<T>(value);

        internal static Result<T> Of(IError error) => new Result<T>(error);
        internal static Result<T> Of(IEnumerable<IError> errors) => new Result<T>(errors);

        public Result<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return IsSuccessful ? selector(Instance) : new Result<TResult>(Errors);
        }

        public Result<TResult> SelectMany<TResult>(Func<T, Result<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return IsSuccessful ? selector(Instance) : new Result<TResult>(Errors);
        }

        public TResult Match<TResult>(Func<T, TResult> success, Func<IReadOnlyList<IError>, TResult> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            return IsSuccessful ? success(Instance) : failure(Errors.ToList());
        }

        public void Match(Action<T> success, Action<IReadOnlyList<IError>> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            if (IsSuccessful)
            {
                success(Instance);
            }
            else
            {
                failure(Errors.ToList());
            }
        }

        public bool Equals(Result<T> other)
        {
            if (!EqualityComparer<T>.Default.Equals(Instance, other.Instance))
            {
                return false;
            }

            if (Errors.Count != other.Errors.Count)
            {
                return false;
            }

            return !Errors.Where((error, i) => !error.Equals(other.Errors[i])).Any();
        }

        public override bool Equals(object obj) =>
            obj is Result<T> other && Equals(other);

        public override int GetHashCode()
        {
            var instanceHashCode = Instance != null ? Instance.GetHashCode() : 0;
            var errorHashCode = Errors.Aggregate(0, (code, error) => code ^ error.GetHashCode());
            return (instanceHashCode * 397) ^ errorHashCode;
        }
    }

    public readonly struct Result : IEquatable<Result>
    {
        private List<IError> Errors { get; }

        private bool IsSuccessful { get; }

        private Result(bool isSuccessful = true)
        {
            Errors = new List<IError>();
            IsSuccessful = isSuccessful;
        }

        private Result(IError error)
        {
            Errors = new List<IError> { error };
            IsSuccessful = false;
        }

        private Result(IEnumerable<IError> error)
        {
            Errors = error.ToList();
            IsSuccessful = false;
        }

        public static implicit operator Result(Success _) => new Result(true);

        internal static Result Of(IError error) => new Result(error);
        internal static Result Of(IEnumerable<IError> errors) => new Result(errors);

        public T Match<T>(Func<T> success, Func<IReadOnlyList<IError>, T> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            return IsSuccessful ? success() : failure(Errors.ToList());
        }

        public void Match(Action success, Action<IReadOnlyList<IError>> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            if (IsSuccessful)
            {
                success();
            }
            else
            {
                failure(Errors.ToList());
            }
        }

        public bool Equals(Result other)
        {
            if (IsSuccessful != other.IsSuccessful)
            {
                return false;
            }

            if (Errors.Count != other.Errors.Count)
            {
                return false;
            }

            return !Errors.Where((error, i) => !error.Equals(other.Errors[i])).Any();
        }

        public override bool Equals(object obj) =>
            obj is Result other && Equals(other);

        public override int GetHashCode() =>
            397 * Errors.Aggregate(0, (code, error) => code ^ error.GetHashCode());
    }

    namespace SuccessInitializer
    {
        public readonly struct Success
        {
            public override int GetHashCode() => 0;

            public override bool Equals(object obj) =>
                obj is Success || obj != null && obj.Equals(this);
        }
    }
}

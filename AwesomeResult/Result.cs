using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeResult.Initializers;

namespace AwesomeResult
{
    public static class Initializer
    {
        public static Failure Failure = default;
    }

    public readonly struct Result<T> : IEquatable<Result<T>> where T : notnull
    {
        private T Instance { get; }

        private List<IError> Errors { get; }

        private Result(T instance)
        {
            Instance = instance;
            Errors = new List<IError>();
        }

        private Result(IError error)
        {
            Instance = default;
            Errors = new List<IError> { error };
        }

        private Result(IEnumerable<IError> errors)
        {
            Instance = default;
            Errors = errors.ToList();
        }

        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator Result<T>(Failure _) => new Result<T>(new List<IError>());

        internal static Result<T> Of(IError error) => new Result<T>(error);
        internal static Result<T> Of(IEnumerable<IError> errors) => new Result<T>(errors);

        public Result<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return EqualityComparer<T>.Default.Equals(Instance, default)
                ? new Result<TResult>(Errors)
                : selector(Instance);
        }

        public Result<TResult> SelectMany<TResult>(Func<T, Result<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return EqualityComparer<T>.Default.Equals(Instance, default)
                ? new Result<TResult>(Errors)
                : selector(Instance);
        }

        public TResult Match<TResult>(Func<T, TResult> success, Func<IReadOnlyList<IError>, TResult> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            return EqualityComparer<T>.Default.Equals(Instance, default)
                ? failure(Errors.ToList())
                : success(Instance);
        }

        public void Switch(Action<T> success, Action<IReadOnlyList<IError>> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            if (EqualityComparer<T>.Default.Equals(Instance, default))
            {
                failure(Errors.ToList());
            }
            else
            {
                success(Instance);
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

    namespace Initializers
    {
        public readonly struct Failure
        {
            public override int GetHashCode() => 0;

            public override bool Equals(object obj) =>
                obj is Failure || obj != null && obj.Equals(this);
        }
    }
}

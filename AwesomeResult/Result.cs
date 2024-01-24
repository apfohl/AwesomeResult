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

    public readonly struct Result<T, TFailure> : IEquatable<Result<T, TFailure>>
        where T : notnull
        where TFailure : notnull
    {
        private T Instance { get; }

        private List<TFailure> Errors { get; }

        private Result(T instance)
        {
            Instance = instance;
            Errors = new List<TFailure>();
        }

        private Result(TFailure error)
        {
            Instance = default;
            Errors = new List<TFailure> { error };
        }

        private Result(IEnumerable<TFailure> errors)
        {
            Instance = default;
            Errors = errors.ToList();
        }

        public static implicit operator Result<T, TFailure>(T value) => new Result<T, TFailure>(value);
        public static implicit operator Result<T, TFailure>(Failure _) => new Result<T, TFailure>(new List<TFailure>());

        internal static Result<T, TFailure> Of(TFailure error) => new Result<T, TFailure>(error);
        internal static Result<T, TFailure> Of(IEnumerable<TFailure> errors) => new Result<T, TFailure>(errors);

        public Result<TResult, TFailure> Select<TResult>(Func<T, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return EqualityComparer<T>.Default.Equals(Instance, default)
                ? new Result<TResult, TFailure>(Errors)
                : selector(Instance);
        }

        public Result<TResult, TFailure> SelectMany<TResult>(Func<T, Result<TResult, TFailure>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return EqualityComparer<T>.Default.Equals(Instance, default)
                ? new Result<TResult, TFailure>(Errors)
                : selector(Instance);
        }

        public TResult Match<TResult>(Func<T, TResult> success, Func<IReadOnlyList<TFailure>, TResult> failure)
        {
            if (success == null) throw new ArgumentNullException(nameof(success));
            if (failure == null) throw new ArgumentNullException(nameof(failure));

            return EqualityComparer<T>.Default.Equals(Instance, default)
                ? failure(Errors.ToList())
                : success(Instance);
        }

        public void Match(Action<T> success, Action<IReadOnlyList<TFailure>> failure)
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

        public bool Equals(Result<T, TFailure> other)
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
            obj is Result<T, TFailure> other && Equals(other);

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

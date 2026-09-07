namespace CCMSApp.Core.Common;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, string? error)
    {
        if (isSuccess && !string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("A successful result cannot have an error message.");

        if (!isSuccess && string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("A failed result must have an error message.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(string error) => new(false, error);
}

/// <summary>
/// Represents the outcome of an operation that returns a value when successful.
/// </summary>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected Result(bool isSuccess, string? error, TValue? value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static Result<TValue> Success(TValue value) => new(true, null, value);

    public static new Result<TValue> Failure(string error) => new(false, error, default);
}

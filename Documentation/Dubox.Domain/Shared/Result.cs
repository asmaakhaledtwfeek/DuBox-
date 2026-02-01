namespace Dubox.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public Error? Error { get; }

    protected internal Result(bool isSuccess, string? message = null, Error? error = null)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("Success result cannot have an error.");
        
        if (!isSuccess && error == null)
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Message = message ?? (isSuccess ? "Success" : "Failure");
        Error = error;
    }

    public static Result Success()
        => new(true);

    public static Result Success(string message)
        => new(true, message);

    public static Result<TValue> Success<TValue>(TValue value)
        => new(value);

    public static Result<TValue> Success<TValue>(TValue value, string message)
        => new(value, true, message);

    public static Result<TValue> Success<TValue>(TValue value, int totalCount)
        => new(value, totalCount);

    public static Result Failure(string message)
        => new(false, message, new Error("Failure", message));

    public static Result Failure(Error error)
        => new(false, error.Description, error);

    public static Result<TValue> Failure<TValue>(string message)
        => new(default, false, message, new Error("Failure", message));

    public static Result<TValue> Failure<TValue>(Error error)
        => new(default, false, error.Description, error);

    public static Result<TValue> Create<TValue>(TValue? value)
        => value is not null 
            ? Success(value) 
            : Failure<TValue>("Value cannot be null");

    public static Result<TValue> Create<TValue>(TValue? value, string errorMessage)
        => value is not null 
            ? Success(value) 
            : Failure<TValue>(errorMessage);

    public static implicit operator bool(Result result) => result.IsSuccess;
}

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified value is null");
}

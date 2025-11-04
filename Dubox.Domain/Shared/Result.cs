namespace Dubox.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    protected internal Result(bool isSuccess, string? message = null)
    {
        IsSuccess = isSuccess;
        Message = message ?? (isSuccess ? "Success" : "Failure");
    }
    public static Result Success()
        => new(true);

    public static Result<TValue> Success<TValue>(TValue value)
        => new(value);

    public static Result<TValue> Success<TValue>(TValue value, int totalCount)
        => new(value, totalCount);
    public static Result Failure(string message)
        => new(false, message);

    public static Result<TValue> Failure<TValue>(string message) =>
       new(default, message);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>("null value");
}

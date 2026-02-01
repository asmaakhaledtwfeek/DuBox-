namespace Dubox.Domain.Shared;

public class Result<TData> : Result
{
    public int? TotalCount { get; }
    public TData? Data { get; }

    protected internal Result(TData data) : base(true)
    {
        Data = data;
    }

    protected internal Result(TData data, int totalCount) : base(true)
    {
        Data = data;
        TotalCount = totalCount;
    }

    protected internal Result(TData? data, bool isSuccess, string message) 
        : base(isSuccess, message, isSuccess ? null : new Error("Failure", message))
    {
        Data = data;
    }

    protected internal Result(TData? data, bool isSuccess, string message, Error error) 
        : base(isSuccess, message, error)
    {
        Data = data;
    }

    public static implicit operator Result<TData>(TData? value) => Create(value);
}

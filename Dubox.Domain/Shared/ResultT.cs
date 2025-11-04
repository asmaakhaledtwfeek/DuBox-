namespace Dubox.Domain.Shared;

public class Result<TData> : Result
{
    public int? TotalCount { get; }

    public TData Data { get; }
    protected internal Result(TData data) : base(true)
    {
        Data = data;
    }
    protected internal Result(TData data, int totalCount) : base(true)
    {
        Data = data;
        TotalCount = totalCount;
    }
    protected internal Result(TData data, string message) : base(false, message)
    {
        Data = data;
    }
    public static implicit operator Result<TData>(TData? value) => Create(value);
}

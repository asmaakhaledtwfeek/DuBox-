namespace Dubox.Application.DTOs;

public record PaginatedRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;


    public (int Page, int PageSize) GetNormalizedPagination()
    {
        var page = Page < 1 ? 1 : Page;
        var pageSize = PageSize < 1 ? 25 : (PageSize > 100 ? 100 : PageSize);
        return (page, pageSize);
    }
}


public record PaginatedResponse<T>
{
    public List<T> Items { get; init; } = new();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}


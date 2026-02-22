using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dubox.Application.Features.CustomReports.Queries;

public class GetCustomReportsQueryHandler : IRequestHandler<GetCustomReportsQuery, Result<List<CustomReportListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomReportsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CustomReportListItemDto>>> Handle(GetCustomReportsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _unitOfWork.Repository<CustomReport>()
                .FindAsync(
                    r => string.IsNullOrWhiteSpace(request.Search) ||
                         r.Name.ToLower().Contains(request.Search.ToLower()),
                    cancellationToken);

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var items = reports
                .OrderByDescending(r => r.CreatedAt)
                .Select(r =>
                {
                    CustomReportConfig? config = null;
                    try { config = JsonSerializer.Deserialize<CustomReportConfig>(r.ConfigJson, jsonOptions); }
                    catch { /* ignore parse errors */ }

                    return new CustomReportListItemDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        DataSource = config?.DataSource ?? string.Empty,
                        ChartType = config?.ChartType ?? "table",
                        ColumnCount = config?.Columns?.Count ?? 0,
                        CreatedAt = r.CreatedAt
                    };
                })
                .ToList();

            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<CustomReportListItemDto>>($"Failed to load reports: {ex.Message}");
        }
    }
}

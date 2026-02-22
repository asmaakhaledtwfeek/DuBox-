using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace Dubox.Application.Features.CustomReports.Queries;

public class GetCustomReportByIdQueryHandler : IRequestHandler<GetCustomReportByIdQuery, Result<CustomReportDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomReportByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CustomReportDto>> Handle(GetCustomReportByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _unitOfWork.Repository<CustomReport>()
                .GetByIdAsync(request.Id, cancellationToken);

            if (report is null)
                return Result.Failure<CustomReportDto>($"Report {request.Id} not found.");

            var config = JsonSerializer.Deserialize<CustomReportConfig>(report.ConfigJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CustomReportConfig();

            return Result.Success(new CustomReportDto
            {
                Id = report.Id,
                Name = report.Name,
                Description = report.Description,
                Config = config,
                CreatedAt = report.CreatedAt,
                UpdatedAt = report.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            return Result.Failure<CustomReportDto>($"Failed to load report: {ex.Message}");
        }
    }
}

using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace Dubox.Application.Features.CustomReports.Commands;

public class SaveCustomReportCommandHandler : IRequestHandler<SaveCustomReportCommand, Result<CustomReportDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public SaveCustomReportCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CustomReportDto>> Handle(SaveCustomReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result.Failure<CustomReportDto>("Report name is required.");

            if (string.IsNullOrWhiteSpace(request.Config?.DataSource))
                return Result.Failure<CustomReportDto>("A data source must be selected.");

            var configJson = JsonSerializer.Serialize(request.Config, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Guid? userId = null;
            if (Guid.TryParse(_currentUserService.UserId, out var uid))
                userId = uid;

            CustomReport report;

            if (request.Id.HasValue && request.Id.Value != Guid.Empty)
            {
                // Update existing report
                report = await _unitOfWork.Repository<CustomReport>()
                    .GetByIdAsync(request.Id.Value, cancellationToken)
                    ?? throw new KeyNotFoundException($"Custom report {request.Id} not found.");

                report.Name = request.Name.Trim();
                report.Description = request.Description?.Trim();
                report.ConfigJson = configJson;
                report.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<CustomReport>().Update(report);
            }
            else
            {
                // Create new report
                report = new CustomReport
                {
                    Name = request.Name.Trim(),
                    Description = request.Description?.Trim(),
                    ConfigJson = configJson,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<CustomReport>().AddAsync(report, cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            var config = JsonSerializer.Deserialize<CustomReportConfig>(report.ConfigJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CustomReportConfig();

            var dto = new CustomReportDto
            {
                Id = report.Id,
                Name = report.Name,
                Description = report.Description,
                Config = config,
                CreatedAt = report.CreatedAt,
                UpdatedAt = report.UpdatedAt
            };

            return Result.Success(dto, request.Id.HasValue ? "Report updated." : "Report created.");
        }
        catch (Exception ex)
        {
            return Result.Failure<CustomReportDto>($"Failed to save report: {ex.Message}");
        }
    }
}

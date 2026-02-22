using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Commands;

/// <summary>
/// Creates a new custom report when Id is null/empty, or updates an existing one.
/// </summary>
public record SaveCustomReportCommand(
    Guid? Id,
    string Name,
    string? Description,
    CustomReportConfig Config
) : IRequest<Result<CustomReportDto>>;

using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

/// <summary>
/// Command to import schedule activities from an Excel file with hierarchical support
/// </summary>
public record ImportScheduleActivitiesFromExcelCommand(
    Stream FileStream,
    string FileName,
    Guid? ProjectId,
    string? SheetName = null
) : IRequest<Result<ScheduleActivityImportResultDto>>;

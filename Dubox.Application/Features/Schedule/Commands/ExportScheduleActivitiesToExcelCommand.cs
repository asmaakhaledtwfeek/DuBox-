using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

public record ExportScheduleActivitiesToExcelCommand(
    Guid ProjectId
) : IRequest<Result<Stream>>;

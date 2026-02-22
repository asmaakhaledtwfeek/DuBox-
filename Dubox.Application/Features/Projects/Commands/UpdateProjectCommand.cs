using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record UpdateProjectCommand(
    Guid ProjectId,
    string? ProjectName,
    string? ClientName,
    string? Description,
    string? BimLink,
    bool? IsActive,
    DateTime? PlannedStartDate,
    int? Duration,
    DateTime? ProjectedEndDate,
    Guid? ProjectMangerId,
    decimal? ProjectValue,
    Guid? ActivityTemplateId,
    Guid? MaterialTemplateId,
    bool? AllowCompletionWithConditionalApproval
) : IRequest<Result<ProjectDto>>;


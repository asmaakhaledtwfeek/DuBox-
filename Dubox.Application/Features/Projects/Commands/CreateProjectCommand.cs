using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record CreateProjectCommand(
    string ProjectCode,
    string ProjectName,
    string? ClientName,
    ProjectLocationEnum Location,
    int? Duration,
    DateTime PlannedStartDate,
    DateTime? PlannedEndtDate,
    DateTime? ProjectedEndDate,
    Guid? ProjectMangerId,
    decimal? ProjectValue,
    string? Description,
    string? BimLink,
    bool AllowCompletionWithConditionalApproval = false,
    Guid? ActivityTemplateId = null,
    Guid? MaterialTemplateId = null
) : IRequest<Result<ProjectDto>>;


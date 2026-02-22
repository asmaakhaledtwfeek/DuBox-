using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public record AssignTemplateToProjectCommand(
    Guid ProjectId,
    Guid MaterialTemplateId,
    /// <summary>
    /// The template that was previously assigned to the project.
    /// When provided, box types whose current template matches this value are eligible for update.
    /// When null, only box types that have no template assigned will receive the new template.
    /// </summary>
    Guid? OldMaterialTemplateId = null
) : IRequest<Result<bool>>;







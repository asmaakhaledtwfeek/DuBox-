using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public record UpdateActivityTemplateCommand : IRequest<Result<Unit>>
{
    public Guid ActivityTemplateId { get; init; }
    public string TemplateName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int StageCount { get; init; } = 1;
    public bool IsActive { get; init; }
    public List<CreateActivityTemplateActivityDto>? Activities { get; init; }
}

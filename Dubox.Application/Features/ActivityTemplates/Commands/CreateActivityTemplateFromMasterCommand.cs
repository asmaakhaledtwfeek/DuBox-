using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

/// <summary>
/// Create an Activity Template by selecting activities from ActivityMaster
/// This command will clone all properties from ActivityMaster
/// </summary>
public record CreateActivityTemplateFromMasterCommand : IRequest<Result<Guid>>
{
    public string TemplateName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int StageCount { get; init; } = 1;
    public List<Guid> ActivityMasterIds { get; init; } = new();
}

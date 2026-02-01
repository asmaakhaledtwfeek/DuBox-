using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record ImportBoxPanelsFromExcelCommand(
    Guid ProjectId,
    Stream FileStream,
    string FileName
) : IRequest<Result<BoxPanelsImportResultDto>>;

public record BoxPanelsImportResultDto
{
    public int SuccessCount { get; init; }
    public int FailureCount { get; init; }
    public List<string> Errors { get; init; } = new();
}

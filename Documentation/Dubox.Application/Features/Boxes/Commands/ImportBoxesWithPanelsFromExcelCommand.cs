using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record ImportBoxesWithPanelsFromExcelCommand(
    Guid ProjectId,
    Stream FileStream,
    string FileName
) : IRequest<Result<BoxWithPanelsImportResultDto>>;


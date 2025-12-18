using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;

namespace Dubox.Application.Features.BoxDrawings.Commands;

public record UploadBoxDrawingCommand(
    Guid BoxId,
    string? DrawingUrl,
    byte[]? File,
    string? FileName
) : IRequest<Result<BoxDrawingDto>>;


using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxDrawings.Commands;

public record UploadBoxDrawingCommand(
    Guid BoxId,
    string? DrawingUrl,
    byte[]? File,
    string? FileName
) : IRequest<Result<BoxDrawingDto>>;


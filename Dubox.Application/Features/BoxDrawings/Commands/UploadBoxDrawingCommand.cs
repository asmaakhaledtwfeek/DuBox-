using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dubox.Application.Features.BoxDrawings.Commands;

public record UploadBoxDrawingCommand(
    Guid BoxId,
    string? DrawingUrl,
    IFormFile? File,
    string? FileName
) : IRequest<Result<BoxDrawingDto>>;


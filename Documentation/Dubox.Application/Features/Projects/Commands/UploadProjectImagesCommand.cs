using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dubox.Application.Features.Projects.Commands;

public record UploadProjectImagesCommand(
    Guid ProjectId,
    IFormFile? ContractorImage,
    IFormFile? SubContractorImage,
    IFormFile? ClientImage
) : IRequest<Result<ProjectDto>>;

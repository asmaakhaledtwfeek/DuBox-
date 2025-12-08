using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Images.Queries;

public record GetImageQuery(ImageCategory Category, Guid ImageId) : IRequest<Result<ImageDataDto>>;


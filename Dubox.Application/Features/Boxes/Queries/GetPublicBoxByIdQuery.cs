using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

/// <summary>
/// Query to get public box information by ID (no authentication required)
/// </summary>
public record GetPublicBoxByIdQuery(Guid BoxId) : IRequest<Result<PublicBoxDto>>;


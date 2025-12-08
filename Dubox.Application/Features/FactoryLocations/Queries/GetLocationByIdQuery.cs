using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public record GetLocationByIdQuery(Guid LocationId) : IRequest<Result<FactoryLocationDto>>;


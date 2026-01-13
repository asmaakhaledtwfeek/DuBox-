using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public record GetAllFactoryLocationsQuery : IRequest<Result<List<FactoryLocationDto>>>;


using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public record CheckLocationExistsQuery(string LocationCode) : IRequest<Result<bool>>;


using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public record GetBoxLocationHistoryQuery(Guid BoxId) : IRequest<Result<List<BoxLocationHistoryDto>>>;


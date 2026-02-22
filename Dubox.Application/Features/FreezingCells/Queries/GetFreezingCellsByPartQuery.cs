using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FreezingCells.Queries;

public record GetFreezingCellsByPartQuery(Guid FactorySectionPartId) : IRequest<Result<IEnumerable<FreezingCellDto>>>;

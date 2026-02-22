using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FreezingCells.Commands;

public record CreateFreezingCellCommand(
    Guid FactorySectionPartId,
    int RowNumber
) : IRequest<Result<FreezingCellDto>>;

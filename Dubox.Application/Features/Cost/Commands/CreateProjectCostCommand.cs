using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Commands;

public record CreateProjectCostCommand(
    Guid BoxId,
    decimal Cost,
    string CostType,
    Guid? HRCostRecordId
) : IRequest<Result<ProjectCostDto>>;




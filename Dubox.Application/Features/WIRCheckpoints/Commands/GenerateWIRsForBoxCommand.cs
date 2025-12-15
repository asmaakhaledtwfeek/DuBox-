using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands;

/// <summary>
/// Command to auto-generate all 6 WIRs (WIR-1 through WIR-6) for a box with predefined checklist items
/// </summary>
public record GenerateWIRsForBoxCommand(
    Guid BoxId
) : IRequest<Result<List<WIRCheckpointDto>>>;

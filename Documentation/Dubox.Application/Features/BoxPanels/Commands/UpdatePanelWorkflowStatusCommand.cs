using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxPanels.Commands;

public record UpdatePanelWorkflowStatusCommand(
    Guid BoxPanelId,
    string WorkflowStatus, // InProgress, Completed, PutOnHold
    PanelStageEnum? CurrentStage, // Stage selection: MoldPreparation, ReinforcementSetup, ConcreteCasting, CuringAndDemolding
    string? Notes,
    Guid? AssignedToTeamId // QC Team for PutOnHold issues
) : IRequest<Result<BoxPanelDto>>;


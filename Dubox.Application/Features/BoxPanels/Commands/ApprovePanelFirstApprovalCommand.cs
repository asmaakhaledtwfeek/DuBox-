using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxPanels.Commands;

public record ApprovePanelFirstApprovalCommand(
    Guid BoxPanelId,
    string ApprovalStatus, // Approved, Rejected
    string? Notes
) : IRequest<Result<BoxPanelDto>>;


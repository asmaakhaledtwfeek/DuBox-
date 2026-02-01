using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands;

public record UpdateChecklistItemCommand(
    Guid ChecklistItemId,
    string? CheckpointDescription,
    string? ReferenceDocument,
    CheckListItemStatusEnum? Status,
    string? Remarks,
    int? Sequence
) : IRequest<Result<WIRChecklistItemDto>>;


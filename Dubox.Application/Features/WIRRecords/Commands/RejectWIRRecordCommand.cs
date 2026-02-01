using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Commands;

public record RejectWIRRecordCommand(
    Guid WIRRecordId,
    string RejectionReason,
    string? InspectionNotes
) : IRequest<Result<WIRRecordDto>>;


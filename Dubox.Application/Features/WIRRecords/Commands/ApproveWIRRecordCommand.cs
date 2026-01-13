using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Commands;

public record ApproveWIRRecordCommand(
    Guid WIRRecordId,
    string? InspectionNotes,
    string? Photo
) : IRequest<Result<WIRRecordDto>>;


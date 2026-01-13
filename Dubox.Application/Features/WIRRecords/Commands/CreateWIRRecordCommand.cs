using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Commands;

public record CreateWIRRecordCommand(
    Guid BoxActivityId,
    string WIRCode,
    string? Photo
) : IRequest<Result<WIRRecordDto>>;


using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Queries;

public record GetWIRRecordByIdQuery(Guid WIRRecordId) : IRequest<Result<WIRRecordDto>>;


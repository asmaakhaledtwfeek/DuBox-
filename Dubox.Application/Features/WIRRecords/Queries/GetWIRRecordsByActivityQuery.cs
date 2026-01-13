using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Queries;

public record GetWIRRecordsByActivityQuery(Guid BoxActivityId) : IRequest<Result<List<WIRRecordDto>>>;


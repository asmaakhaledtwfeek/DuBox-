using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Queries;

public record GetWIRRecordsByBoxQuery(Guid BoxId) : IRequest<Result<List<WIRRecordDto>>>;


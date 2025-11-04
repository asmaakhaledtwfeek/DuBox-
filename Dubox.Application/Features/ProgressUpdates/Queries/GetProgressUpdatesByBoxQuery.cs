using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public record GetProgressUpdatesByBoxQuery(Guid BoxId) : IRequest<Result<List<ProgressUpdateDto>>>;


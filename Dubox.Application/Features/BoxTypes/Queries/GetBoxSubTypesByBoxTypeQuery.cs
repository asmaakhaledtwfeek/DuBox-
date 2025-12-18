using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypes.Queries;

public record GetBoxSubTypesByBoxTypeQuery(int BoxTypeId) : IRequest<Result<List<BoxSubTypeDto>>>;


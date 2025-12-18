using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypes.Queries;

public record GetBoxTypesByCategoryQuery(int CategoryId) : IRequest<Result<List<BoxTypeDto>>>;


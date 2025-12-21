using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Factories.Queries;

public record GetFactoryByIdQuery(Guid FactoryId) : IRequest<Result<FactoryDto>>;


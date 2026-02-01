using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Factories.Queries;

public record GetAllFactoriesQuery : IRequest<Result<List<FactoryDto>>>;


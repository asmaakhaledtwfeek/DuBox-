using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Factories.Queries;

public record GetFactoriesByLocationQuery(ProjectLocationEnum Location) : IRequest<Result<List<FactoryDto>>>;


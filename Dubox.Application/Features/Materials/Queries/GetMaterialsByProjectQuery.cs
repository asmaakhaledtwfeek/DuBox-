using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries;

public record GetMaterialsByProjectQuery(Guid ProjectId) : IRequest<Result<List<MaterialDto>>>;







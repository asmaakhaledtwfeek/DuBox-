using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public record GetAvailableMaterialsForTemplateQuery(
    string? SearchTerm = null,
    string? Category = null
) : IRequest<Result<List<MaterialDto>>>;







using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Queries;

public record GetAllActivityTemplatesQuery(
    bool ActiveOnly = true,
    string? SearchTerm = null
) : IRequest<Result<List<ActivityTemplateDto>>>;

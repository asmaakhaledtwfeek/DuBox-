using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Queries;

public record GetActivityTemplateByIdQuery(Guid ActivityTemplateId) : IRequest<Result<ActivityTemplateDetailsDto>>;

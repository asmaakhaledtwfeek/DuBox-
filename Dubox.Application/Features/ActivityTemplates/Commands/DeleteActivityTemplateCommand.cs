using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public record DeleteActivityTemplateCommand(Guid ActivityTemplateId) : IRequest<Result<Unit>>;

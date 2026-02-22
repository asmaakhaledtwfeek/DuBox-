using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public record DeleteMaterialTemplateCommand(Guid MaterialTemplateId) : IRequest<Result<bool>>;







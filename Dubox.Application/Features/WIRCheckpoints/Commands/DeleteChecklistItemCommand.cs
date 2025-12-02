using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands;

public record DeleteChecklistItemCommand(Guid ChecklistItemId) : IRequest<Result<bool>>;


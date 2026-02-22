using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Commands;

public record CreateChecklistCommand(
    string Name,
    string Code,
    string Discipline,
    string? SubDiscipline,
    int PageNumber,
    string? WIRCode,
    List<string>? ReferenceDocuments,
    List<string>? SignatureRoles
) : IRequest<Result<Guid>>;

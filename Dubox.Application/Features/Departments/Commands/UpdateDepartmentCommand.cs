using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Departments.Commands
{
    public record UpdateDepartmentCommand(
    Guid DepartmentId,
    string? DepartmentName,
    string? Code,
    string? Description,
    string? Location,
    bool? IsActive,
    Guid? ManagerId
) : IRequest<Result<DepartmentDto>>;
}

using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Departments.Commands
{
    public record DeleteDepartmentCommand(Guid DepartmentId) : IRequest<Result>;
}

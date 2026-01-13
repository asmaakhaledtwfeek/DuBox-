using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Departments.Queries
{
    public record GetAllDepartmentsQuery : IRequest<Result<List<DepartmentDto>>>;
}

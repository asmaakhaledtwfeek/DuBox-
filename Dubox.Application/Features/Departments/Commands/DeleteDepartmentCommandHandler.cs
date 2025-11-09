using Dubox.Application.Features.Departments.Commands;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _unitOfWork.Repository<Department>()
            .GetByIdAsync(request.DepartmentId, cancellationToken);

        if (department == null)
            return Result.Failure("Department not found.");

        var hasUsers = department.Employees != null && department.Employees.Count > 0;

        if (hasUsers)
            return Result.Failure("Cannot delete department with assigned employees.");

        _unitOfWork.Repository<Department>().Delete(department);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}


using Dubox.Application.DTOs;
using Dubox.Application.Features.Departments.Commands;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

public class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, Result<DepartmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DepartmentDto>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _unitOfWork.Repository<Department>()
            .GetByIdAsync(request.DepartmentId, cancellationToken);

        if (department == null)
            return Result.Failure<DepartmentDto>("Department not found.");

        if (!string.IsNullOrEmpty(request.Code))
        {
            var codeExists = await _unitOfWork.Repository<Department>()
                .IsExistAsync(d => d.Code == request.Code && d.DepartmentId != request.DepartmentId, cancellationToken);

            if (codeExists)
                return Result.Failure<DepartmentDto>("A department with this code already exists.");

            department.Code = request.Code;
        }

        if (!string.IsNullOrEmpty(request.DepartmentName))
        {
            var nameExists = await _unitOfWork.Repository<Department>()
                .IsExistAsync(d => d.DepartmentName == request.DepartmentName && d.DepartmentId != request.DepartmentId, cancellationToken);

            if (nameExists)
                return Result.Failure<DepartmentDto>("A department with this name already exists.");

            department.DepartmentName = request.DepartmentName;
        }
        ApplyDepartmentUpdates(department, request);

        department.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Repository<Department>().Update(department);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(department.Adapt<DepartmentDto>());
    }

    private void ApplyDepartmentUpdates(Department department, UpdateDepartmentCommand request)
    {
        if (!string.IsNullOrEmpty(request.Description))
            department.Description = request.Description;

        if (!string.IsNullOrEmpty(request.Location))
            department.Location = request.Location;

        if (request.ManagerId.HasValue)
            department.ManagerId = request.ManagerId.Value;

        if (request.IsActive.HasValue)
            department.IsActive = request.IsActive.Value;
    }
}


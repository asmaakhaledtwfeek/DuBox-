using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Roles.Commands;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>()
            .GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
            return Result.Failure<RoleDto>("Role not found.");

        // Check if role name is being changed and if the new name already exists
        if (!string.IsNullOrEmpty(request.RoleName) && role.RoleName != request.RoleName)
        {
            var nameExists = await _unitOfWork.Repository<Role>()
                .IsExistAsync(r => r.RoleName == request.RoleName && r.RoleId != request.RoleId, cancellationToken);

            if (nameExists)
                return Result.Failure<RoleDto>("Role with this name already exists.");
        }

        // Update role properties
        if (!string.IsNullOrEmpty(request.RoleName))
            role.RoleName = request.RoleName;

        if (request.Description != null)
            role.Description = request.Description;

        role.IsActive = request.IsActive;

        _unitOfWork.Repository<Role>().Update(role);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(role.Adapt<RoleDto>());
    }
}

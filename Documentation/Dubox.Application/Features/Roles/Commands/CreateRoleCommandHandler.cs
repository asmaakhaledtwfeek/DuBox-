using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Roles.Commands;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleExists = await _unitOfWork.Repository<Role>()
            .IsExistAsync(r => r.RoleName == request.RoleName, cancellationToken);

        if (roleExists)
            return Result.Failure<RoleDto>("Role with this name already exists");

        var role = new Role
        {
            RoleName = request.RoleName,
            Description = request.Description,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Role>().AddAsync(role, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(role.Adapt<RoleDto>());
    }
}


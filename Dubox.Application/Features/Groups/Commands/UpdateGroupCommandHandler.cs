using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Groups.Commands;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Result<GroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public UpdateGroupCommandHandler(IUnitOfWork unitOfWork, IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<GroupDto>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Repository<Group>()
            .GetByIdAsync(request.GroupId, cancellationToken);

        if (group == null)
            return Result.Failure<GroupDto>("Group not found.");

        // Check if group name is being changed and if the new name already exists
        if (!string.IsNullOrEmpty(request.GroupName) && group.GroupName != request.GroupName)
        {
            var nameExists = await _unitOfWork.Repository<Group>()
                .IsExistAsync(g => g.GroupName == request.GroupName && g.GroupId != request.GroupId, cancellationToken);

            if (nameExists)
                return Result.Failure<GroupDto>("Group with this name already exists.");
        }

        // Update group properties
        if (!string.IsNullOrEmpty(request.GroupName))
            group.GroupName = request.GroupName;

        if (request.Description != null)
            group.Description = request.Description;

        group.IsActive = request.IsActive;

        _unitOfWork.Repository<Group>().Update(group);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Reload group with roles for the response
        var updatedGroup = await _context.Groups
            .Include(g => g.GroupRoles)
                .ThenInclude(gr => gr.Role)
            .FirstOrDefaultAsync(g => g.GroupId == request.GroupId, cancellationToken);

        if (updatedGroup == null)
            return Result.Failure<GroupDto>("Failed to retrieve updated group.");

        var groupDto = new GroupDto
        {
            GroupId = updatedGroup.GroupId,
            GroupName = updatedGroup.GroupName,
            Description = updatedGroup.Description,
            IsActive = updatedGroup.IsActive,
            CreatedDate = updatedGroup.CreatedDate,
            Roles = updatedGroup.GroupRoles.Select(gr => gr.Role.Adapt<RoleDto>()).ToList()
        };

        return Result.Success(groupDto);
    }
}

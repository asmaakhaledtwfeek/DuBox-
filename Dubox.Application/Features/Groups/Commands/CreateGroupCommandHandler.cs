using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<GroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GroupDto>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var groupExists = await _unitOfWork.Repository<Group>()
            .IsExistAsync(g => g.GroupName == request.GroupName, cancellationToken);

        if (groupExists)
            return Result.Failure<GroupDto>("Group with this name already exists");

        var group = new Group
        {
            GroupName = request.GroupName,
            Description = request.Description,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Group>().AddAsync(group, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(group.Adapt<GroupDto>());
    }
}


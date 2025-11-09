using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class UpdateBoxCommandHandler : IRequestHandler<UpdateBoxCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoxCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BoxDto>> Handle(UpdateBoxCommand request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        if (!string.IsNullOrEmpty(request.BoxTag) && box.BoxTag != request.BoxTag)
        {
            var tagExists = await _unitOfWork.Repository<Box>()
                .IsExistAsync(b => b.ProjectId == box.ProjectId && b.BoxTag == request.BoxTag && b.BoxId != request.BoxId, cancellationToken);

            if (tagExists)
                return Result.Failure<BoxDto>("Box tag already exists in the project");

            box.BoxTag = request.BoxTag;

        }
        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId, cancellationToken);
        box.QRCodeString = $"{project!.ProjectCode}_{request.BoxTag}";
        ApplyBoxUpdates(box, request);

        box.ModifiedDate = DateTime.UtcNow;
        _unitOfWork.Repository<Box>().Update(box);
        await _unitOfWork.CompleteAsync(cancellationToken);
        BoxDto response = box.Adapt<BoxDto>();
        response.ProjectCode = project.ProjectCode;
        return Result.Success(response);
    }

    private void ApplyBoxUpdates(Box box, UpdateBoxCommand request)
    {
        if (!string.IsNullOrEmpty(request.BoxName))
            box.BoxName = request.BoxName;

        if (!string.IsNullOrEmpty(request.BoxType))
            box.BoxType = request.BoxType;

        if (!string.IsNullOrEmpty(request.Floor))
            box.Floor = request.Floor;

        if (!string.IsNullOrEmpty(request.Building))
            box.Building = request.Building;

        if (!string.IsNullOrEmpty(request.Zone))
            box.Zone = request.Zone;

        if (!string.IsNullOrEmpty(request.Notes))
            box.Notes = request.Notes;

        if (request.Length.HasValue)
            box.Length = request.Length.Value;

        if (request.Width.HasValue)
            box.Width = request.Width.Value;

        if (request.Height.HasValue)
            box.Height = request.Height.Value;

        if (request.Status.HasValue)
            box.Status = (BoxStatusEnum)request.Status.Value;

        if (request.PlannedEndDate.HasValue)
            box.PlannedEndDate = request.PlannedEndDate.Value;
    }
}


using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
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

        // Check if box tag changed and is unique
        if (box.BoxTag != request.BoxTag)
        {
            var tagExists = await _unitOfWork.Repository<Box>()
                .IsExistAsync(b => b.ProjectId == box.ProjectId && b.BoxTag == request.BoxTag && b.BoxId != request.BoxId, cancellationToken);

            if (tagExists)
                return Result.Failure<BoxDto>("Box tag already exists in the project");

            // Update QR code string if tag changed
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId, cancellationToken);
            box.QRCodeString = $"{project!.ProjectCode}_{request.BoxTag}";
        }

        box.BoxTag = request.BoxTag;
        box.BoxName = request.BoxName;
        box.BoxType = request.BoxType;
        box.Floor = request.Floor;
        box.Building = request.Building;
        box.Zone = request.Zone;
        box.Status = request.Status;
        box.Length = request.Length;
        box.Width = request.Width;
        box.Height = request.Height;
        box.PlannedEndDate = request.PlannedEndDate;
        box.Notes = request.Notes;
        box.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<Box>().Update(box);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(box.Adapt<BoxDto>());
    }
}


using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Commands;

public class CreateBoxCommandHandler : IRequestHandler<CreateBoxCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public CreateBoxCommandHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<BoxDto>> Handle(CreateBoxCommand request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<BoxDto>("Project not found");

        // Check if box already exists
        var boxExists = await _unitOfWork.Repository<Box>()
            .IsExistAsync(b => b.ProjectId == request.ProjectId && b.BoxTag == request.BoxTag, cancellationToken);

        if (boxExists)
            return Result.Failure<BoxDto>("Box with this tag already exists in the project");

        // Create QR Code String
        var qrCodeString = $"{project.ProjectCode}_{request.BoxTag}";

        var box = new Box
        {
            ProjectId = request.ProjectId,
            BoxTag = request.BoxTag,
            BoxName = request.BoxName,
            BoxType = request.BoxType,
            Floor = request.Floor,
            Building = request.Building,
            Zone = request.Zone,
            QRCodeString = qrCodeString,
            QRCodeImageUrl = null,
            ProgressPercentage = 0,
            Status = "Not Started",
            Length = request.Length,
            Width = request.Width,
            Height = request.Height,
            UnitOfMeasure = "mm",
            BIMModelReference = request.BIMModelReference,
            RevitElementId = request.RevitElementId,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Box>().AddAsync(box, cancellationToken);

        // Auto-copy activities
        var activityMasters = await _dbContext.ActivityMasters
            .Where(am => am.IsActive)
            .OrderBy(am => am.OverallSequence)
            .ToListAsync(cancellationToken);

        foreach (var activityMaster in activityMasters)
        {
            if (!string.IsNullOrEmpty(activityMaster.ApplicableBoxTypes))
            {
                var applicableTypes = activityMaster.ApplicableBoxTypes.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (!applicableTypes.Any(t => t.Trim().Equals(request.BoxType, StringComparison.OrdinalIgnoreCase)))
                    continue;
            }

            var boxActivity = new BoxActivity
            {
                BoxId = box.BoxId,
                ActivityMasterId = activityMaster.ActivityMasterId,
                Sequence = activityMaster.OverallSequence,
                Status = "Not Started",
                ProgressPercentage = 0,
                MaterialsAvailable = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<BoxActivity>().AddAsync(boxActivity, cancellationToken);
        }

        // Add assets if provided
        if (request.Assets != null && request.Assets.Any())
        {
            foreach (var assetDto in request.Assets)
            {
                var asset = new BoxAsset
                {
                    BoxId = box.BoxId,
                    AssetType = assetDto.AssetType,
                    AssetCode = assetDto.AssetCode,
                    AssetName = assetDto.AssetName,
                    Quantity = assetDto.Quantity,
                    Unit = assetDto.Unit,
                    Specifications = assetDto.Specifications,
                    Notes = assetDto.Notes,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<BoxAsset>().AddAsync(asset, cancellationToken);
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Update project total boxes
        project.TotalBoxes++;
        _unitOfWork.Repository<Project>().Update(project);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(box.Adapt<BoxDto>());
    }
}


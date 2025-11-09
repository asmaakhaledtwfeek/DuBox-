using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Commands;

public class CreateBoxCommandHandler : IRequestHandler<CreateBoxCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateBoxCommandHandler(IUnitOfWork unitOfWork, IDbContext dbContext, IMapper Mapper)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _mapper = Mapper;
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

        var box = _mapper.Map<Box>(request);
        box.QRCodeString = $"{project.ProjectCode}_{request.BoxTag}"; ;
        await _unitOfWork.Repository<Box>().AddAsync(box, cancellationToken);

        // Auto-copy activities
        var boxType = request.BoxType?.Trim();

        var activityMasters = await _dbContext.ActivityMasters
            .Where(am => am.IsActive &&
                (string.IsNullOrEmpty(am.ApplicableBoxTypes) ||
                 am.ApplicableBoxTypes
                   .Split(',', StringSplitOptions.RemoveEmptyEntries)
                   .Any(t => t.Trim().Equals(boxType, StringComparison.OrdinalIgnoreCase))))
            .OrderBy(am => am.OverallSequence)
            .ToListAsync(cancellationToken);

        var boxActivities = activityMasters.Select(am => new BoxActivity
        {
            BoxId = box.BoxId,
            ActivityMasterId = am.ActivityMasterId,
            Sequence = am.OverallSequence,
            Status = "Not Started",
            ProgressPercentage = 0,
            MaterialsAvailable = true,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        await _unitOfWork.Repository<BoxActivity>().AddRangeAsync(boxActivities, cancellationToken);



        //// Add assets if provided
        //if (request.Assets != null && request.Assets.Any())
        //{
        //    foreach (var assetDto in request.Assets)
        //    {
        //        var asset = new BoxAsset
        //        {
        //            BoxId = box.BoxId,
        //            AssetType = assetDto.AssetType,
        //            AssetCode = assetDto.AssetCode,
        //            AssetName = assetDto.AssetName,
        //            Quantity = assetDto.Quantity,
        //            Unit = assetDto.Unit,
        //            Specifications = assetDto.Specifications,
        //            Notes = assetDto.Notes,
        //            CreatedDate = DateTime.UtcNow
        //        };

        //        await _unitOfWork.Repository<BoxAsset>().AddAsync(asset, cancellationToken);
        //    }
        //}

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Update project total boxes
        project.TotalBoxes++;
        _unitOfWork.Repository<Project>().Update(project);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(box.Adapt<BoxDto>());
    }
}


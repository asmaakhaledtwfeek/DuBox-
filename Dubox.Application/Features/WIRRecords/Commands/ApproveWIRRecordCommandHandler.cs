using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRRecords.Commands;

public class ApproveWIRRecordCommandHandler : IRequestHandler<ApproveWIRRecordCommand, Result<WIRRecordDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ApproveWIRRecordCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<WIRRecordDto>> Handle(ApproveWIRRecordCommand request, CancellationToken cancellationToken)
    {
        var wirRecord = _unitOfWork.Repository<WIRRecord>().
           GetEntityWithSpec(new GetWIRRecordWIthIncludesSpecification(request.WIRRecordId));
        if (wirRecord == null)
            return Result.Failure<WIRRecordDto>("WIR record not found");

        if (wirRecord.Status == WIRRecordStatusEnum.Approved)
            return Result.Failure<WIRRecordDto>("WIR record is already approved");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var inspector = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);

        if (inspector == null)
            return Result.Failure<WIRRecordDto>("Inspector user not found");

        wirRecord.Status = WIRRecordStatusEnum.Approved;
        wirRecord.InspectedBy = currentUserId;
        wirRecord.InspectionDate = DateTime.UtcNow;
        wirRecord.InspectionNotes = request.InspectionNotes;

        if (!string.IsNullOrEmpty(request.Photo))
        {
            wirRecord.Photo = string.IsNullOrEmpty(wirRecord.Photo)
                ? request.Photo
                : $"{wirRecord.Photo},{request.Photo}";
        }

        wirRecord.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<WIRRecord>().Update(wirRecord);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = wirRecord.Adapt<WIRRecordDto>() with
        {
            BoxId = wirRecord.BoxActivity.Box.BoxId,
            BoxTag = wirRecord.BoxActivity.Box.BoxTag,
            ActivityName = wirRecord.BoxActivity.ActivityMaster.ActivityName,
            RequestedByName = wirRecord.RequestedByUser.FullName ?? wirRecord.RequestedByUser.Email,
            InspectedByName = inspector.FullName ?? inspector.Email
        };

        return Result.Success(dto);
    }
}


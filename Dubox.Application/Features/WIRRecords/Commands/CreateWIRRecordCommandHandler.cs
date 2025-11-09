using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRRecords.Commands;

public class CreateWIRRecordCommandHandler : IRequestHandler<CreateWIRRecordCommand, Result<WIRRecordDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateWIRRecordCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<WIRRecordDto>> Handle(CreateWIRRecordCommand request, CancellationToken cancellationToken)
    {
        // Verify box activity exists
        var boxActivity = await _dbContext.BoxActivities
            .Include(ba => ba.ActivityMaster)
            .Include(ba => ba.Box)
            .FirstOrDefaultAsync(ba => ba.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (boxActivity == null)
            return Result.Failure<WIRRecordDto>("Box activity not found");

        // Check if WIR already exists for this activity
        var existingWir = await _dbContext.WIRRecords
            .FirstOrDefaultAsync(w => w.BoxActivityId == request.BoxActivityId, cancellationToken);

        if (existingWir != null)
            return Result.Failure<WIRRecordDto>("WIR record already exists for this activity");

        // Get current user
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);

        if (user == null)
            return Result.Failure<WIRRecordDto>("User not found");

        var wirRecord = new WIRRecord
        {
            BoxActivityId = request.BoxActivityId,
            WIRCode = request.WIRCode,
            Status = "Pending",
            RequestedDate = DateTime.UtcNow,
            RequestedBy = currentUserId,
            PhotoUrls = request.PhotoUrls,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<WIRRecord>().AddAsync(wirRecord, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = wirRecord.Adapt<WIRRecordDto>() with
        {
            BoxTag = boxActivity.Box.BoxTag,
            ActivityName = boxActivity.ActivityMaster.ActivityName,
            RequestedByName = user.FullName ?? user.Email
        };

        return Result.Success(dto);
    }
}


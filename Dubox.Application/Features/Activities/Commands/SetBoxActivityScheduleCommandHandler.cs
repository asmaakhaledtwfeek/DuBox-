using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands
{


    public class SetBoxActivityScheduleCommandHandler : IRequestHandler<SetBoxActivityScheduleCommand, Result<BoxActivityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public SetBoxActivityScheduleCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<BoxActivityDto>> Handle(SetBoxActivityScheduleCommand request, CancellationToken cancellationToken)
        {
            var activity = _unitOfWork.Repository<BoxActivity>()
                .GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.ActivityId));

            if (activity == null)
                return Result.Failure<BoxActivityDto>("Box Activity not found.");
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            const string dateFormat = "yyyy-MM-dd HH:mm:ss";

            var oldPlannedStartDateString = activity.PlannedStartDate?.ToString(dateFormat) ?? "N/A";
            var oldPlannedEndDateString = activity.PlannedEndDate?.ToString(dateFormat) ?? "N/A";
            var oldDuration = activity.Duration;

            activity.PlannedStartDate = request.PlannedStartDate;
            activity.Duration = request.Duration;
            activity.PlannedEndDate = request.PlannedStartDate.AddDays(request.Duration);
            activity.ModifiedBy = currentUserId;
            activity.ModifiedDate = DateTime.UtcNow;

            _unitOfWork.Repository<BoxActivity>().Update(activity);
            var newPlannedStartDateString = activity.PlannedStartDate?.ToString(dateFormat) ?? "N/A";
            var newPlannedEndDateString = activity.PlannedEndDate?.ToString(dateFormat) ?? "N/A";
            var newDuration = activity.Duration;


            var log = new AuditLog
            {
                TableName = nameof(BoxActivity),
                RecordId = activity.BoxActivityId,
                Action = "ScheduleUpdate",
                OldValues = $"Start: {oldPlannedStartDateString}, End: {oldPlannedEndDateString}, Duration: {oldDuration}",
                NewValues = $"Start: {newPlannedStartDateString}, End: {newPlannedEndDateString}, Duration: {newDuration}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = "Activity planned schedule and duration updated."
            };

            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(activity.Adapt<BoxActivityDto>());
        }
    }
}

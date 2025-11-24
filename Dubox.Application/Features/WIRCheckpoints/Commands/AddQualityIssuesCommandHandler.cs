using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class AddQualityIssuesCommandHandler
    : IRequestHandler<AddQualityIssuesCommand, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public AddQualityIssuesCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(AddQualityIssuesCommand request, CancellationToken cancellationToken)
        {
            var wir = _unitOfWork.Repository<WIRCheckpoint>().
                GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));


            if (wir is null)
                return Result.Failure<WIRCheckpointDto>("WIRCheckpoint not found.");
            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var parsedUserId)
              ? parsedUserId
              : Guid.Empty;
            var reportedBy = string.Empty;
            if (currentUserId != Guid.Empty)
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);
                if (user != null)
                {
                    reportedBy = user.FullName;
                }
            }
            var existingIssues = await _unitOfWork.Repository<QualityIssue>().FindAsync(x => x.WIRId == request.WIRId);
            if (existingIssues.Any())
                _unitOfWork.Repository<QualityIssue>().DeleteRange(existingIssues);
            var newIssues = request.Issues.Select(i => new QualityIssue
            {
                WIRId = wir.WIRId,
                BoxId = wir.BoxId,
                IssueType = i.IssueType,
                Severity = i.Severity,
                IssueDescription = i.IssueDescription,
                AssignedTo = i.AssignedTo,
                DueDate = i.DueDate,
                PhotoPath = i.PhotoPath,
                Status = QualityIssueStatusEnum.Open,
                IssueDate = DateTime.UtcNow,
                ReportedBy = reportedBy
            }).ToList();

            await _unitOfWork.Repository<QualityIssue>().AddRangeAsync(newIssues, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = wir.Adapt<WIRCheckpointDto>();

            return Result.Success(dto);
        }
    }

}

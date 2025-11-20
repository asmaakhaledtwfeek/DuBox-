using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public class UpdateQualityIssueStatusCommandHandler : IRequestHandler<UpdateQualityIssueStatusCommand, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateQualityIssueStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(UpdateQualityIssueStatusCommand request, CancellationToken cancellationToken)
        {
            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue == null)
                return Result.Failure<QualityIssueDetailsDto>("Quality issue not found.");
            issue.Status = request.Status;

            if (request.Status == QualityIssueStatusEnum.Resolved ||
                request.Status == QualityIssueStatusEnum.Closed)
            {
                issue.ResolutionDescription = request.ResolutionDescription;
                issue.ResolutionDate = DateTime.UtcNow;
            }
            else
            {
                issue.ResolutionDescription = null;
                issue.ResolutionDate = null;
            }

            if (!string.IsNullOrWhiteSpace(request.PhotoPath))
                issue.PhotoPath = request.PhotoPath;

            _unitOfWork.Repository<QualityIssue>().Update(issue);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = issue.Adapt<QualityIssueDetailsDto>();

            return Result.Success(dto);
        }
    }

}

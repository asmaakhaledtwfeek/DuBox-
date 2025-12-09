using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssueByIdQueryHandler : IRequestHandler<GetQualityIssueByIdQuery, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetQualityIssueByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(GetQualityIssueByIdQuery request, CancellationToken cancellationToken)
        {
            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue is null)
                return Result.Failure<QualityIssueDetailsDto>("Quality Issue not found.");

            // Verify user has access to the project this quality issue belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(issue.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to view this quality issue.");
            }

            var dto = issue.Adapt<QualityIssueDetailsDto>();
            dto.Images = issue.Images
                .OrderBy(img => img.Sequence)
                .Select(img => new QualityIssueImageDto
                {
                    QualityIssueImageId = img.QualityIssueImageId,
                    IssueId = img.IssueId,
                    ImageData = img.ImageData,
                    ImageType = img.ImageType,
                    OriginalName = img.OriginalName,
                    FileSize = img.FileSize,
                    Sequence = img.Sequence,
                    CreatedDate = img.CreatedDate
                }).ToList();

            return Result.Success(dto);
        }
    }

}

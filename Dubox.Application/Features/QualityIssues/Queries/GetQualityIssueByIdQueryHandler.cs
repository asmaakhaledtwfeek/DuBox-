using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssueByIdQueryHandler : IRequestHandler<GetQualityIssueByIdQuery, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IQualityIssueMappingService _mappingService;

        public GetQualityIssueByIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IProjectTeamVisibilityService visibilityService,
            IBlobStorageService blobStorageService,
            IQualityIssueMappingService mappingService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
            _blobStorageService = blobStorageService;
            _mappingService = mappingService;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(GetQualityIssueByIdQuery request, CancellationToken cancellationToken)
        {
            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue is null)
                return Result.Failure<QualityIssueDetailsDto>("Quality Issue not found.");

            // Get project ID from either Box or direct Project
            var projectId = issue.Box?.ProjectId ?? issue.ProjectId;
            if (!projectId.HasValue)
            {
                return Result.Failure<QualityIssueDetailsDto>("Project ID not found for this quality issue.");
            }

            // Verify user has access to the project this quality issue belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(projectId.Value, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<QualityIssueDetailsDto>("Access denied. You do not have permission to view this quality issue.");
            }

            // Use mapping service to convert issue to DTO with images
            var dto = _mappingService.MapToDto(issue, includeImages: true, blobStorageService: _blobStorageService);

            return Result.Success(dto);
        }
    }

}

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
    public class GetQualityIssuesByBoxIdQueryHandler : IRequestHandler<GetQualityIssuesByBoxIdQuery, Result<List<QualityIssueDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetQualityIssuesByBoxIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
        }

        public async Task<Result<List<QualityIssueDetailsDto>>> Handle(GetQualityIssuesByBoxIdQuery request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
            if (box == null)
                return Result.Failure<List<QualityIssueDetailsDto>>("Box not found");

            // Verify user has access to the project this box belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<List<QualityIssueDetailsDto>>("Access denied. You do not have permission to view quality issues for this box.");
            }

            var specificationResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesByBoxIdSpecification(request.BoxId));
            var issues = specificationResult.Data.ToList();

            var dtos = issues.Select(issue =>
            {
                var dto = issue.Adapt<QualityIssueDetailsDto>();
                dto.AssignedToUserName =!string.IsNullOrEmpty(issue.AssignedToMember?.EmployeeName)? issue.AssignedToMember?.EmployeeName: issue.AssignedToMember?.User.FullName;
                dto.CCUserName = !string.IsNullOrEmpty(issue.CCUser?.FullName) ? issue.CCUser?.FullName :string.Empty;
                // Map project information from Box.Project
                if (issue.Box?.Project != null)
                {
                    dto.ProjectId = issue.Box.Project.ProjectId;
                    dto.ProjectName = issue.Box.Project.ProjectName;
                    dto.ProjectCode = issue.Box.Project.ProjectCode;
                }
                
                return dto;
            }).ToList();

            return Result.Success(dtos);
        }
    }

}

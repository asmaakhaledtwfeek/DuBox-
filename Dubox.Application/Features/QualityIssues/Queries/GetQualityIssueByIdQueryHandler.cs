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
        private readonly IBlobStorageService _blobStorageService;
        private const string _containerName = "images";
        public GetQualityIssueByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService ,IBlobStorageService blobStorageService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
            _blobStorageService = blobStorageService;
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
            dto.AssignedToUserName = !string.IsNullOrEmpty(issue.AssignedToMember?.EmployeeName)? issue.AssignedToMember?.EmployeeName:issue.AssignedToMember.User.FullName;
            dto.CCUserName = issue.CCUser?.FullName;
            
            // Map project information from Box.Project
            if (issue.Box?.Project != null)
            {
                dto.ProjectId = issue.Box.Project.ProjectId;
                dto.ProjectName = issue.Box.Project.ProjectName;
                dto.ProjectCode = issue.Box.Project.ProjectCode;
            }
            
            dto.Images = issue.Images
           .OrderBy(img => img.Sequence)
           .Select(img => new QualityIssueImageDto
           {
               QualityIssueImageId = img.QualityIssueImageId,
               IssueId = img.IssueId,
               ImageFileName = img.ImageFileName,
               ImageUrl = !string.IsNullOrEmpty(img.ImageFileName)
                   ? _blobStorageService.GetImageUrl(_containerName, img.ImageFileName)
                   : null,
               ImageType = img.ImageType,
               OriginalName = img.OriginalName,
               FileSize = img.FileSize,
               Sequence = img.Sequence,
               Version = img.Version,
               CreatedDate = img.CreatedDate
           }).ToList();

            return Result.Success(dto);
        }
    }

}

using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssueFiltersQueryHandler : IRequestHandler<GetQualityIssueFiltersQuery, Result<QualityIssueFiltersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetQualityIssueFiltersQueryHandler(
            IUnitOfWork unitOfWork,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
        }

        public async Task<Result<QualityIssueFiltersDto>> Handle(GetQualityIssueFiltersQuery request, CancellationToken cancellationToken)
        {
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            // Get QualityIssues query
            var issuesQuery = _unitOfWork.Repository<QualityIssue>().GetWithSpec(new GetQualityIssuesSpecification(accessibleProjectIds))
               .Data.ToList();


            // Get distinct issue numbers
            var issueNumbers =  issuesQuery
                .Select(qi => qi.IssueNumber)
                .Distinct()
                .OrderBy(i => i)
                .ToList();

            // Get distinct box tags (exclude null boxes)
            var boxTags =  issuesQuery
                .Where(qi => qi.Box != null && !string.IsNullOrEmpty(qi.Box.BoxTag))
                .Select(qi => qi.Box.BoxTag)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            // Get distinct project codes (from Box.Project or direct Project)
            var projectCodes = issuesQuery
                .Where(qi => 
                    (qi.Box != null && qi.Box.Project != null && !string.IsNullOrEmpty(qi.Box.Project.ProjectCode)) ||
                    (qi.Project != null && !string.IsNullOrEmpty(qi.Project.ProjectCode)))
                .Select(qi => qi.Box != null && qi.Box.Project != null 
                    ? qi.Box.Project.ProjectCode 
                    : qi.Project.ProjectCode)
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            // Get box tags with their project codes for cascading filter (only for box-level issues)
            var boxTagsWithProjects = issuesQuery
                .Where(qi => qi.Box != null && 
                            qi.Box.Project != null && 
                            !string.IsNullOrEmpty(qi.Box.Project.ProjectCode) &&
                            !string.IsNullOrEmpty(qi.Box.BoxTag))
                .Select(qi => new BoxTagWithProject
                {
                    BoxTag = qi.Box.BoxTag,
                    ProjectCode = qi.Box.Project.ProjectCode
                })
                .Distinct()
                .OrderBy(b => b.ProjectCode)
                .ThenBy(b => b.BoxTag)
                .ToList();

            var result = new QualityIssueFiltersDto
            {
                IssueNumbers = issueNumbers,
                BoxTags = boxTags,
                ProjectCodes = projectCodes,
                BoxTagsWithProjects = boxTagsWithProjects
            };

            return Result.Success(result);
        }
    }
}

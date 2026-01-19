using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesQueryHandler : IRequestHandler<GetQualityIssuesQuery, Result<PaginatedQualityIssuesResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IProjectTeamVisibilityService _visibilityService;
        
        public GetQualityIssuesQueryHandler(
            IUnitOfWork unitOfWork, 
            IDbContext dbContext,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _visibilityService = visibilityService;
        }

        public async Task<Result<PaginatedQualityIssuesResponseDto>> Handle(GetQualityIssuesQuery request, CancellationToken cancellationToken)
        {
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            
            var (page, pageSize) = new PaginatedRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            }.GetNormalizedPagination();
            
            var qualityIssuesResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesSpecification(request, accessibleProjectIds));
            
            var qualityIssues = await qualityIssuesResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            var totalCount = qualityIssuesResult.Count;

            var dtos = qualityIssues.Select(issue =>
            {
                var dto = issue.Adapt<QualityIssueDetailsDto>();
                dto.AssignedToUserName =!string.IsNullOrEmpty(issue.AssignedToMember?.EmployeeName)? issue.AssignedToMember?.EmployeeName: issue.AssignedToMember?.User.FullName;
                
                // Map project information from Box.Project
                if (issue.Box?.Project != null)
                {
                    dto.ProjectId = issue.Box.Project.ProjectId;
                    dto.ProjectName = issue.Box.Project.ProjectName;
                    dto.ProjectCode = issue.Box.Project.ProjectCode;
                }
                
                return dto;
            }).ToList();
            

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Calculate summary statistics for ALL quality issues (not just current page)
            var summary = await CalculateSummaryAsync(request, accessibleProjectIds, cancellationToken);

           var response = new PaginatedQualityIssuesResponseDto
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Summary = summary
            };

            return Result.Success(response);
        }

        private async Task<QualityIssuesSummary> CalculateSummaryAsync(
            GetQualityIssuesQuery request, 
            List<Guid>? accessibleProjectIds, 
            CancellationToken cancellationToken)
        {
            var allIssuesResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesSummarySpecification(request, accessibleProjectIds));

            var statusCounts = await allIssuesResult.Data
                .AsNoTracking()
                .GroupBy(i => i.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var summary = new QualityIssuesSummary
            {
                TotalIssues = statusCounts.Sum(s => s.Count),
                OpenIssues = statusCounts
                    .Where(s => s.Status == Domain.Enums.QualityIssueStatusEnum.Open)
                    .Sum(s => s.Count),
                InProgressIssues = statusCounts
                    .Where(s => s.Status == Domain.Enums.QualityIssueStatusEnum.InProgress)
                    .Sum(s => s.Count),
                ResolvedIssues = statusCounts
                    .Where(s => s.Status == Domain.Enums.QualityIssueStatusEnum.Resolved)
                    .Sum(s => s.Count),
                ClosedIssues = statusCounts
                    .Where(s => s.Status == Domain.Enums.QualityIssueStatusEnum.Closed)
                    .Sum(s => s.Count)
            };

            return summary;
        }
        
       
    }

}

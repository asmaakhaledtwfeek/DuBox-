using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetBoxPanelApprovalsReportQueryHandler : IRequestHandler<GetBoxPanelApprovalsReportQuery, Result<List<BoxPanelApprovalReportDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxPanelApprovalsReportQueryHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<BoxPanelApprovalReportDto>>> Handle(
        GetBoxPanelApprovalsReportQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            // Build query for boxes with panels
            var query = _dbContext.Boxes
                .Include(b => b.Project)
                .Include(b => b.BoxPanels)
                .Where(b => accessibleProjectIds.Contains(b.ProjectId))
                .Where(b => b.BoxPanels != null && b.BoxPanels.Count > 0)
                .AsQueryable();

            // Apply project filter
            if (request.ProjectId.HasValue)
            {
                query = query.Where(b => b.ProjectId == request.ProjectId.Value);
            }

            // Apply factory filter
            if (request.FactoryId.HasValue)
            {
                query = query.Where(b => b.FactoryId == request.FactoryId.Value);
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchLower = request.Search.ToLower();
                query = query.Where(b =>
                    (b.BoxTag != null && b.BoxTag.ToLower().Contains(searchLower)) ||
                    (b.BoxName != null && b.BoxName.ToLower().Contains(searchLower)) ||
                    (b.Project != null && b.Project.ProjectName != null && b.Project.ProjectName.ToLower().Contains(searchLower)));
            }

            // Execute query
            var boxes = await query
                .OrderBy(b => b.Project!.ProjectName)
                .ThenBy(b => b.BoxTag)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var result = boxes.Select(b =>
            {
                var totalPanels = b.BoxPanels?.Count ?? 0;
                var approvedPanels = b.BoxPanels?.Count(p =>
                    p.SecondApprovalStatus != null &&
                    p.SecondApprovalStatus.ToLower() == "approved") ?? 0;
                var approvalPercentage = totalPanels > 0 ? (decimal)approvedPanels / totalPanels * 100 : 0;
                var allPanelsApproved = approvedPanels == totalPanels && totalPanels > 0;

                return new BoxPanelApprovalReportDto
                {
                    BoxId = b.BoxId,
                    BoxCode = b.BoxTag ?? string.Empty,
                    BoxName = b.BoxName ?? string.Empty,
                    ProjectId = b.ProjectId,
                    ProjectName = b.Project?.ProjectName ?? "Unknown Project",
                    ProjectNumber = b.Project?.ProjectCode ?? string.Empty,
                    TotalPanels = totalPanels,
                    ApprovedPanels = approvedPanels,
                    ApprovalPercentage = Math.Round(approvalPercentage, 2),
                    AllPanelsApproved = allPanelsApproved
                };
            }).ToList();

            // Apply approval status filter after DTO mapping (since it's calculated)
            if (!string.IsNullOrWhiteSpace(request.ApprovalStatus) && request.ApprovalStatus.ToLower() != "all")
            {
                if (request.ApprovalStatus.ToLower() == "approved")
                {
                    result = result.Where(r => r.AllPanelsApproved).ToList();
                }
                else if (request.ApprovalStatus.ToLower() == "pending")
                {
                    result = result.Where(r => !r.AllPanelsApproved).ToList();
                }
            }

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<BoxPanelApprovalReportDto>>($"Error generating box panel approvals report: {ex.Message}");
        }
    }
}


using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetProjectPanelTypeApprovalsReportQueryHandler : IRequestHandler<GetProjectPanelTypeApprovalsReportQuery, Result<List<ProjectPanelTypeApprovalReportDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetProjectPanelTypeApprovalsReportQueryHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<ProjectPanelTypeApprovalReportDto>>> Handle(
        GetProjectPanelTypeApprovalsReportQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            // Build query for panels with grouping
            var query = _dbContext.BoxPanels
                .Include(p => p.Box)
                    .ThenInclude(b => b!.Project)
                .Include(p => p.PanelType)
                .Where(p => p.Box != null && accessibleProjectIds.Contains(p.Box.ProjectId))
                .AsQueryable();

            // Apply project filter
            if (request.ProjectId.HasValue)
            {
                query = query.Where(p => p.Box!.ProjectId == request.ProjectId.Value);
            }

            // Apply factory filter
            if (request.FactoryId.HasValue)
            {
                query = query.Where(p => p.Box!.FactoryId == request.FactoryId.Value);
            }

            // Apply panel type filter
            if (!string.IsNullOrWhiteSpace(request.PanelType))
            {
                query = query.Where(p => p.PanelType != null && p.PanelType.PanelTypeName == request.PanelType);
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchLower = request.Search.ToLower();
                query = query.Where(p =>
                    (p.Box!.Project!.ProjectName != null && p.Box.Project.ProjectName.ToLower().Contains(searchLower)) ||
                    (p.PanelType != null && p.PanelType.PanelTypeName.ToLower().Contains(searchLower)));
            }

            // Execute query
            var panels = await query.ToListAsync(cancellationToken);

            // Group by project and panel type
            var groupedData = panels
                .GroupBy(p => new
                {
                    ProjectId = p.Box!.ProjectId,
                    ProjectName = p.Box.Project!.ProjectName,
                    ProjectNumber = p.Box.Project.ProjectCode,
                    PanelType = p.PanelType?.PanelTypeName ?? p.PanelType?.PanelTypeCode ?? "Unknown"
                })
                .Select(g =>
                {
                    var totalPanels = g.Count();
                    var approvedPanels = g.Count(p =>
                        p.SecondApprovalStatus != null &&
                        p.SecondApprovalStatus.ToLower() == "approved");
                    var pendingPanels = totalPanels - approvedPanels;
                    var approvalPercentage = totalPanels > 0 ? (decimal)approvedPanels / totalPanels * 100 : 0;

                    return new ProjectPanelTypeApprovalReportDto
                    {
                        ProjectId = g.Key.ProjectId,
                        ProjectName = g.Key.ProjectName ?? "Unknown Project",
                        ProjectNumber = g.Key.ProjectNumber ?? string.Empty,
                        PanelType = g.Key.PanelType,
                        TotalPanels = totalPanels,
                        ApprovedPanels = approvedPanels,
                        PendingPanels = pendingPanels,
                        ApprovalPercentage = Math.Round(approvalPercentage, 2)
                    };
                })
                .OrderBy(r => r.ProjectName)
                .ThenBy(r => r.PanelType)
                .ToList();

            return Result.Success(groupedData);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<ProjectPanelTypeApprovalReportDto>>($"Error generating project panel type approvals report: {ex.Message}");
        }
    }
}


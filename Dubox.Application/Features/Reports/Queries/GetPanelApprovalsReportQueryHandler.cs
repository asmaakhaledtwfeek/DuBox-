using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetPanelApprovalsReportQueryHandler : IRequestHandler<GetPanelApprovalsReportQuery, Result<PanelApprovalReportResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetPanelApprovalsReportQueryHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _visibilityService = visibilityService;
    }

    public async Task<Result<PanelApprovalReportResponseDto>> Handle(
        GetPanelApprovalsReportQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Apply visibility filtering
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            var hasAccessRestrictions = accessibleProjectIds != null && accessibleProjectIds.Any();

            // Build query for panels with joins
            var query = from p in _dbContext.BoxPanels
                        join b in _dbContext.Boxes on p.BoxId equals b.BoxId
                        join proj in _dbContext.Projects on b.ProjectId equals proj.ProjectId
                        join pt in _dbContext.PanelTypes on p.PanelTypeId equals pt.PanelTypeId into panelTypes
                        from pt in panelTypes.DefaultIfEmpty()
                        join u in _dbContext.Users on p.SecondApprovalBy equals u.UserId into users
                        from u in users.DefaultIfEmpty()
                        // Only apply visibility filter if user has restricted access (not system admin)
                        where !hasAccessRestrictions || accessibleProjectIds.Contains(b.ProjectId)
                        select new
                        {
                            Panel = p,
                            Box = b,
                            Project = proj,
                            PanelType = pt,
                            ApprovedByUser = u
                        };

            // Apply project filter
            if (request.ProjectId.HasValue)
            {
                query = query.Where(x => x.Box.ProjectId == request.ProjectId.Value);
            }

            // Apply factory filter
            if (request.FactoryId.HasValue)
            {
                query = query.Where(x => x.Box.FactoryId == request.FactoryId.Value);
            }

            // Apply panel type filter
            if (!string.IsNullOrWhiteSpace(request.PanelType))
            {
                query = query.Where(x => x.PanelType != null && x.PanelType.PanelTypeName == request.PanelType);
            }

            // Apply approval status filter
            if (!string.IsNullOrWhiteSpace(request.ApprovalStatus) && request.ApprovalStatus.ToLower() != "all")
            {
                var status = request.ApprovalStatus.ToLower();
                
                if (status == "first-approval")
                {
                    query = query.Where(x => x.Panel.FirstApprovalDate.HasValue);
                }
                else if (status == "second-approval")
                {
                    query = query.Where(x => x.Panel.SecondApprovalStatus != null && 
                                           x.Panel.SecondApprovalStatus.ToLower() == "approved");
                }
                else if (status == "rejected")
                {
                    query = query.Where(x => x.Panel.FirstApprovalStatus != null && 
                                           x.Panel.FirstApprovalStatus.ToLower() == "rejected" ||
                                           x.Panel.SecondApprovalStatus != null && 
                                           x.Panel.SecondApprovalStatus.ToLower() == "rejected");
                }
                // Support legacy "approved" and "pending" for backward compatibility
                else if (status == "approved")
                {
                    query = query.Where(x => x.Panel.SecondApprovalStatus != null && 
                                           x.Panel.SecondApprovalStatus.ToLower() == "approved");
                }
                else if (status == "pending")
                {
                    query = query.Where(x => x.Panel.SecondApprovalStatus == null || 
                                           x.Panel.SecondApprovalStatus.ToLower() != "approved");
                }
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchLower = request.Search.ToLower();
                query = query.Where(x =>
                    (x.Panel.PanelName != null && x.Panel.PanelName.ToLower().Contains(searchLower)) ||
                    (x.Box.BoxTag != null && x.Box.BoxTag.ToLower().Contains(searchLower)) ||
                    (x.Project.ProjectName != null && x.Project.ProjectName.ToLower().Contains(searchLower)) ||
                    (x.PanelType != null && x.PanelType.PanelTypeName.ToLower().Contains(searchLower)) ||
                    (x.ApprovedByUser != null && x.ApprovedByUser.FullName != null && x.ApprovedByUser.FullName.ToLower().Contains(searchLower)));
            }

            // Execute query
            var results = await query
                .OrderBy(x => x.Project.ProjectName)
                .ThenBy(x => x.Box.BoxTag)
                .ThenBy(x => x.Panel.PanelName)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var panelDtos = results.Select(x => new PanelApprovalReportDto
            {
                PanelId = x.Panel.BoxPanelId,
                PanelName = x.Panel.PanelName ?? "Unknown",
                PanelType = x.PanelType?.PanelTypeName ?? "Unknown",
                BoxCode = x.Box.BoxTag ?? string.Empty,
                BoxName = x.Box.BoxName ?? string.Empty,
                ProjectId = x.Box.ProjectId,
                ProjectName = x.Project.ProjectName ?? "Unknown Project",
                ProjectNumber = x.Project.ProjectCode ?? string.Empty,
                FirstApprovalDate = x.Panel.FirstApprovalDate,
                SecondApprovalDate = x.Panel.SecondApprovalDate,
                SecondApprovalBy = x.ApprovedByUser?.FullName ?? (x.Panel.SecondApprovalBy.HasValue ? "Unknown User" : null),
                Status = (x.Panel.SecondApprovalStatus != null && x.Panel.SecondApprovalStatus.ToLower() == "approved") 
                    ? "Approved" 
                    : (x.Panel.SecondApprovalStatus ?? "Pending")
            }).ToList();

            // Group panels by panel type
            var groupedByPanelType = results
                .GroupBy(x => new
                {
                    PanelTypeId = x.PanelType?.PanelTypeId,
                    PanelTypeName = x.PanelType?.PanelTypeName ?? "Unknown"
                })
                .Select(g =>
                {
                    var totalPanels = g.Count();
                    var firstApprovalCount = g.Count(x => x.Panel.FirstApprovalDate.HasValue);
                    var secondApprovalCount = g.Count(x =>
                        x.Panel.SecondApprovalStatus != null &&
                        x.Panel.SecondApprovalStatus.ToLower() == "approved");
                    var rejectedCount = g.Count(x =>
                        (x.Panel.FirstApprovalStatus != null && x.Panel.FirstApprovalStatus.ToLower() == "rejected") ||
                        (x.Panel.SecondApprovalStatus != null && x.Panel.SecondApprovalStatus.ToLower() == "rejected"));
                    var pendingFirstApproval = totalPanels - firstApprovalCount;
                    var pendingSecondApproval = totalPanels - secondApprovalCount;
                    var boxesUsingThisType = g.Select(x => x.Box.BoxId).Distinct().Count();
                    var firstApprovalPercentage = totalPanels > 0 ? (decimal)firstApprovalCount / totalPanels * 100 : 0;
                    var secondApprovalPercentage = totalPanels > 0 ? (decimal)secondApprovalCount / totalPanels * 100 : 0;
                    var projectNames = g.Select(x => x.Project.ProjectName).Distinct().OrderBy(p => p).ToList();

                    return new PanelTypeGroupedReportDto
                    {
                        PanelType = g.Key.PanelTypeName,
                        PanelTypeId = g.Key.PanelTypeId,
                        TotalPanels = totalPanels,
                        FirstApprovalCount = firstApprovalCount,
                        SecondApprovalCount = secondApprovalCount,
                        RejectedCount = rejectedCount,
                        PendingFirstApproval = pendingFirstApproval,
                        PendingSecondApproval = pendingSecondApproval,
                        BoxesUsingThisType = boxesUsingThisType,
                        FirstApprovalPercentage = Math.Round(firstApprovalPercentage, 2),
                        SecondApprovalPercentage = Math.Round(secondApprovalPercentage, 2),
                        ProjectNames = projectNames
                    };
                })
                .OrderBy(x => x.PanelType)
                .ToList();

            // Calculate summary statistics
            var totalPanels = results.Count;
            var firstApprovalCount = results.Count(x => x.Panel.FirstApprovalDate.HasValue);
            var secondApprovalCount = results.Count(x => 
                x.Panel.SecondApprovalStatus != null && 
                x.Panel.SecondApprovalStatus.ToLower() == "approved");
            var pendingFirstApproval = totalPanels - firstApprovalCount;
            var pendingSecondApproval = totalPanels - secondApprovalCount;
            var totalBoxes = results.Select(x => x.Box.BoxId).Distinct().Count();
            var totalPanelTypes = results
                .Where(x => x.PanelType != null)
                .Select(x => x.PanelType!.PanelTypeId)
                .Distinct()
                .Count();
            var firstApprovalPercentage = totalPanels > 0 ? (decimal)firstApprovalCount / totalPanels * 100 : 0;
            var secondApprovalPercentage = totalPanels > 0 ? (decimal)secondApprovalCount / totalPanels * 100 : 0;

            var summary = new PanelApprovalReportSummaryDto
            {
                TotalPanels = totalPanels,
                FirstApprovalCount = firstApprovalCount,
                SecondApprovalCount = secondApprovalCount,
                PendingFirstApproval = pendingFirstApproval,
                PendingSecondApproval = pendingSecondApproval,
                TotalBoxes = totalBoxes,
                TotalPanelTypes = totalPanelTypes,
                FirstApprovalPercentage = Math.Round(firstApprovalPercentage, 2),
                SecondApprovalPercentage = Math.Round(secondApprovalPercentage, 2)
            };

            var response = new PanelApprovalReportResponseDto
            {
                Panels = panelDtos,
                GroupedByPanelType = groupedByPanelType,
                Summary = summary
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PanelApprovalReportResponseDto>($"Error generating panel approvals report: {ex.Message}");
        }
    }
}


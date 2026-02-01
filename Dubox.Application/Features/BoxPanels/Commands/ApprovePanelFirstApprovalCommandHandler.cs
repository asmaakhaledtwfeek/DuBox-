using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxPanels.Commands;

public class ApprovePanelFirstApprovalCommandHandler : IRequestHandler<ApprovePanelFirstApprovalCommand, Result<BoxPanelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ApprovePanelFirstApprovalCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxPanelDto>> Handle(ApprovePanelFirstApprovalCommand request, CancellationToken cancellationToken)
    {
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
            .FirstOrDefaultAsync(p => p.BoxPanelId == request.BoxPanelId, cancellationToken);

        if (panel == null)
            return Result.Failure<BoxPanelDto>("Panel not found");

        // Check if box is dispatched
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<BoxPanelDto>("Cannot approve panel. Box is dispatched and read-only.");

        // First approval only when panel is completed: both workflow and panel status must be Completed
        if (panel.WorkflowStatus != "Completed" || panel.PanelStatus != PanelStatusEnum.Completed)
            return Result.Failure<BoxPanelDto>("Panel must complete workflow (status: Completed) before first approval.");

        // Check if there's an open quality issue
        if (panel.QualityIssueId.HasValue)
        {
            var issue = await _unitOfWork.Repository<QualityIssue>().GetByIdAsync(panel.QualityIssueId.Value, cancellationToken);
            if (issue != null && issue.Status != QualityIssueStatusEnum.Resolved && issue.Status != QualityIssueStatusEnum.Closed)
            {
                return Result.Failure<BoxPanelDto>($"Cannot approve panel. Quality issue {issue.IssueNumber} must be resolved first. Issue must be resolved by the creator or admin.");
            }
        }

        // Validate approval status
        if (request.ApprovalStatus != "Approved" && request.ApprovalStatus != "Rejected")
            return Result.Failure<BoxPanelDto>("Invalid approval status. Must be 'Approved' or 'Rejected'");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var approvalTime = DateTime.UtcNow;

        panel.FirstApprovalStatus = request.ApprovalStatus;
        panel.FirstApprovalBy = currentUserId;
        panel.FirstApprovalDate = approvalTime;
        panel.FirstApprovalNotes = request.Notes;

        // Update panel status based on approval
        if (request.ApprovalStatus == "Approved")
        {
            panel.PanelStatus = PanelStatusEnum.FirstApprovalApproved;
            panel.FirstApprovalStatus = "Approved";
            // Auto-set to second approval pending
            panel.SecondApprovalStatus = "Pending";
           
        }
        else
        {
            panel.PanelStatus = PanelStatusEnum.Rejected;
            panel.FirstApprovalStatus = "Rejected";
            panel.CurrentLocationStatus = "Rejected";
            panel.WorkflowStatus = "Rejected"; // Send back to workflow
            
            // Automatically create quality issue for rejected panel - assign to QC Team by default
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);
            var reportedBy = user?.FullName ?? "System";

            var qcTeam = await _dbContext.Teams
                .Where(t => t.IsActive && (
                    (t.TeamCode != null && (t.TeamCode.ToLower() == "qc" || t.TeamCode.ToLower() == "qc-team")) ||
                    (t.TeamName != null && (t.TeamName.ToLower().Contains("qc") || t.TeamName.ToLower().Contains("quality control")))))
                .OrderBy(t => t.TeamCode ?? "")
                .FirstOrDefaultAsync(cancellationToken);
            var assignedToTeamId = qcTeam?.TeamId;

            // Generate issue number
            var issueCountInProject = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesSpecification()).Data
                .Count(qi => qi.Box.ProjectId == panel.ProjectId);
            var issueNumber = (issueCountInProject + 1).ToString("D5");
            
            var description = $"Panel '{panel.PanelName}' rejected at First Approval (Pre-cast Location). {request.Notes}";
            
            var newIssue = new QualityIssue
            {
                IssueNumber = issueNumber,
                BoxId = panel.BoxId,
                IssueType = IssueTypeEnum.Defect,
                Severity = SeverityEnum.Major,
                IssueDescription = description,
                NCR = NCRTypeEnum.Internal,
                Status = QualityIssueStatusEnum.Open,
                IssueDate = approvalTime,
                ReportedBy = reportedBy,
                CreatedBy = currentUserId,
                AssignedToTeamId = assignedToTeamId, // Assign to QC Team by default
                CreatedDate = approvalTime
            };
            
            await _unitOfWork.Repository<QualityIssue>().AddAsync(newIssue, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken); // Save to get IssueId
            
            // Link issue to panel
            panel.QualityIssueId = newIssue.IssueId;
        }

        panel.ModifiedDate = approvalTime;
        panel.ModifiedBy = currentUserId;

        _unitOfWork.Repository<BoxPanel>().Update(panel);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = new BoxPanelDto
        {
            BoxPanelId = panel.BoxPanelId,
            BoxId = panel.BoxId,
            ProjectId = panel.ProjectId,
            PanelName = panel.PanelName,
            PanelStatus = panel.PanelStatus,
            QualityIssueId = panel.QualityIssueId,
            CreatedDate = panel.CreatedDate,
            ModifiedDate = panel.ModifiedDate
        };

        return Result.Success(dto);
    }
}


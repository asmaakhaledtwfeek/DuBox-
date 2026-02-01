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

public class ApprovePanelSecondApprovalCommandHandler : IRequestHandler<ApprovePanelSecondApprovalCommand, Result<BoxPanelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ApprovePanelSecondApprovalCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxPanelDto>> Handle(ApprovePanelSecondApprovalCommand request, CancellationToken cancellationToken)
    {
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
            .FirstOrDefaultAsync(p => p.BoxPanelId == request.BoxPanelId, cancellationToken);

        if (panel == null)
            return Result.Failure<BoxPanelDto>("Panel not found");

        // Check if box is dispatched
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<BoxPanelDto>("Cannot approve panel. Box is dispatched and read-only.");

        // Validate that first approval is done
        if (panel.FirstApprovalStatus != "Approved")
            return Result.Failure<BoxPanelDto>("First approval must be completed before second approval");

        // Validate approval status
        if (request.ApprovalStatus != "Approved" && request.ApprovalStatus != "Rejected")
            return Result.Failure<BoxPanelDto>("Invalid approval status. Must be 'Approved' or 'Rejected'");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var approvalTime = DateTime.UtcNow;

        panel.SecondApprovalStatus = request.ApprovalStatus;
        panel.SecondApprovalBy = currentUserId;
        panel.SecondApprovalDate = approvalTime;
        panel.SecondApprovalNotes = request.Notes;

        // Update panel status based on approval
        if (request.ApprovalStatus == "Approved")
        {
            panel.PanelStatus = PanelStatusEnum.SecondApprovalApproved;
            // Panel is now ready for installation (GREEN with checkmark)
        }
        else
        {
            panel.PanelStatus = PanelStatusEnum.SecondApprovalRejected;
            panel.CurrentLocationStatus = "Rejected";
            
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
            
            var description = $"Panel '{panel.PanelName}' rejected at Second Approval (Dubox Delivery). {request.Notes}";
            
            var newIssue = new QualityIssue
            {
                IssueNumber = issueNumber,
                BoxId = panel.BoxId,
                IssueType = IssueTypeEnum.Defect,
                Severity = SeverityEnum.Critical,
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

        // Check if all panels in the box are SecondApprovalApproved and update box status to ReadyToStart
        if (request.ApprovalStatus == "Approved" && panel.Box.Status == BoxStatusEnum.NotStarted)
        {
            // Get all OTHER panels for this box (excluding current panel)
            var otherPanelsInBox = await _dbContext.BoxPanels
                .Where(bp => bp.BoxId == panel.BoxId && bp.BoxPanelId != panel.BoxPanelId)
                .ToListAsync(cancellationToken);

            // Check if all other panels are also SecondApprovalApproved
            // Current panel is already updated to SecondApprovalApproved above
            var allOtherPanelsApproved = !otherPanelsInBox.Any() || 
                otherPanelsInBox.All(bp => bp.PanelStatus == PanelStatusEnum.SecondApprovalApproved);

            if (allOtherPanelsApproved)
            {
                // All panels including current one are approved, update box status
                var box = await _unitOfWork.Repository<Box>().GetByIdAsync(panel.BoxId, cancellationToken);
                if (box != null && box.Status == BoxStatusEnum.NotStarted)
                {
                    box.Status = BoxStatusEnum.ReadyToStart;
                    box.ModifiedDate = approvalTime;
                    box.ModifiedBy = currentUserId;
                    _unitOfWork.Repository<Box>().Update(box);
                }
            }
        }

        // Save all changes (panel + box if applicable) in one transaction
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


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

public class UpdatePanelWorkflowStatusCommandHandler : IRequestHandler<UpdatePanelWorkflowStatusCommand, Result<BoxPanelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public UpdatePanelWorkflowStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

  

    public async Task<Result<BoxPanelDto>> Handle(UpdatePanelWorkflowStatusCommand request, CancellationToken cancellationToken)
    {
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
                .ThenInclude(b => b.Project)
            .FirstOrDefaultAsync(p => p.BoxPanelId == request.BoxPanelId, cancellationToken);

        if (panel == null)
            return Result.Failure<BoxPanelDto>("Panel not found");

        // Check if box is dispatched
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<BoxPanelDto>("Cannot update panel workflow. Box is dispatched and read-only.");

        // Validate workflow status
        var validStatuses = new[] { "InProgress", "Completed", "PutOnHold" };
        if (!validStatuses.Contains(request.WorkflowStatus))
            return Result.Failure<BoxPanelDto>($"Invalid workflow status. Must be one of: {string.Join(", ", validStatuses)}");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var statusTime = DateTime.UtcNow;

        // Get previous status for tracking
        var previousStatus = panel.WorkflowStatus;

        // Update workflow status
        panel.WorkflowStatus = request.WorkflowStatus;
        panel.WorkflowStatusDate = statusTime;
        panel.WorkflowStatusBy = currentUserId;
        panel.WorkflowStatusNotes = request.Notes;

        // Update current stage if provided
        if (request.CurrentStage.HasValue && request.CurrentStage.Value != PanelStageEnum.NotStarted)
        {
            panel.CurrentStage = request.CurrentStage.Value;

            // Mark stages as complete based on current stage (all 7 stages)
            switch (request.CurrentStage.Value)
            {
                case PanelStageEnum.MoldPreparation:
                    if (!panel.MoldPreparationComplete)
                    {
                        panel.MoldPreparationComplete = true;
                        panel.MoldPreparationDate = statusTime;
                        
                        // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    break;
                    
                case PanelStageEnum.Initial:
                    // Check if Mold Preparation is being marked complete for the first time
                    var moldPrepWasIncomplete = !panel.MoldPreparationComplete;
                    panel.MoldPreparationComplete = true;
                    if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
                    
                    // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                    if (moldPrepWasIncomplete)
                    {
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    
                    if (!panel.InitialComplete)
                    {
                        panel.InitialComplete = true;
                        panel.InitialDate = statusTime;
                    }
                    break;
                    
                case PanelStageEnum.MEPInsertsInstallation:
                    var moldPrepWasIncompleteMEP = !panel.MoldPreparationComplete;
                    panel.MoldPreparationComplete = true;
                    panel.InitialComplete = true;
                    if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
                    if (!panel.InitialDate.HasValue) panel.InitialDate = statusTime;
                    
                    // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                    if (moldPrepWasIncompleteMEP)
                    {
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    
                    if (!panel.MEPInsertsInstallationComplete)
                    {
                        panel.MEPInsertsInstallationComplete = true;
                        panel.MEPInsertsInstallationDate = statusTime;
                    }
                    break;
                    
                case PanelStageEnum.ReinforcementSetup:
                    var moldPrepWasIncompleteReinforcement = !panel.MoldPreparationComplete;
                    panel.MoldPreparationComplete = true;
                    panel.InitialComplete = true;
                    panel.MEPInsertsInstallationComplete = true;
                    if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
                    if (!panel.InitialDate.HasValue) panel.InitialDate = statusTime;
                    if (!panel.MEPInsertsInstallationDate.HasValue) panel.MEPInsertsInstallationDate = statusTime;
                    
                    // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                    if (moldPrepWasIncompleteReinforcement)
                    {
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    
                    if (!panel.ReinforcementSetupComplete)
                    {
                        panel.ReinforcementSetupComplete = true;
                        panel.ReinforcementSetupDate = statusTime;
                    }
                    break;
                    
                case PanelStageEnum.ConcreteCasting:
                    var moldPrepWasIncompleteCasting = !panel.MoldPreparationComplete;
                    panel.MoldPreparationComplete = true;
                    panel.InitialComplete = true;
                    panel.MEPInsertsInstallationComplete = true;
                    panel.ReinforcementSetupComplete = true;
                    if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
                    if (!panel.InitialDate.HasValue) panel.InitialDate = statusTime;
                    if (!panel.MEPInsertsInstallationDate.HasValue) panel.MEPInsertsInstallationDate = statusTime;
                    if (!panel.ReinforcementSetupDate.HasValue) panel.ReinforcementSetupDate = statusTime;
                    
                    // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                    if (moldPrepWasIncompleteCasting)
                    {
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    
                    if (!panel.ConcreteCastingComplete)
                    {
                        panel.ConcreteCastingComplete = true;
                        panel.ConcreteCastingDate = statusTime;
                    }
                    break;
                    
                case PanelStageEnum.SurfaceFinishing:
                    var moldPrepWasIncompleteSurface = !panel.MoldPreparationComplete;
                    panel.MoldPreparationComplete = true;
                    panel.InitialComplete = true;
                    panel.MEPInsertsInstallationComplete = true;
                    panel.ReinforcementSetupComplete = true;
                    panel.ConcreteCastingComplete = true;
                    if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
                    if (!panel.InitialDate.HasValue) panel.InitialDate = statusTime;
                    if (!panel.MEPInsertsInstallationDate.HasValue) panel.MEPInsertsInstallationDate = statusTime;
                    if (!panel.ReinforcementSetupDate.HasValue) panel.ReinforcementSetupDate = statusTime;
                    if (!panel.ConcreteCastingDate.HasValue) panel.ConcreteCastingDate = statusTime;
                    
                    // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                    if (moldPrepWasIncompleteSurface)
                    {
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    
                    if (!panel.SurfaceFinishingComplete)
                    {
                        panel.SurfaceFinishingComplete = true;
                        panel.SurfaceFinishingDate = statusTime;
                    }
                    break;
                    
                case PanelStageEnum.CuringAndDemolding:
                    var moldPrepWasIncompleteCuring = !panel.MoldPreparationComplete;
                    panel.MoldPreparationComplete = true;
                    panel.InitialComplete = true;
                    panel.MEPInsertsInstallationComplete = true;
                    panel.ReinforcementSetupComplete = true;
                    panel.ConcreteCastingComplete = true;
                    panel.SurfaceFinishingComplete = true;
                    if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
                    if (!panel.InitialDate.HasValue) panel.InitialDate = statusTime;
                    if (!panel.MEPInsertsInstallationDate.HasValue) panel.MEPInsertsInstallationDate = statusTime;
                    if (!panel.ReinforcementSetupDate.HasValue) panel.ReinforcementSetupDate = statusTime;
                    if (!panel.ConcreteCastingDate.HasValue) panel.ConcreteCastingDate = statusTime;
                    if (!panel.SurfaceFinishingDate.HasValue) panel.SurfaceFinishingDate = statusTime;
                    
                    // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
                    if (moldPrepWasIncompleteCuring)
                    {
                        await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
                    }
                    
                    if (!panel.CuringAndDemoldingComplete)
                    {
                        panel.CuringAndDemoldingComplete = true;
                        panel.CuringAndDemoldingDate = statusTime;
                    }
                    break;
            }
        }

        // If status is Completed, mark all 7 stages complete and prepare for approval
        if (request.WorkflowStatus == "Completed")
        {
            // Check if Mold Preparation is being marked complete for the first time
            var moldPrepWasIncompleteCompleted = !panel.MoldPreparationComplete;
            
            // Ensure all 7 stages are marked complete
            panel.MoldPreparationComplete = true;
            panel.InitialComplete = true;
            panel.MEPInsertsInstallationComplete = true;
            panel.ReinforcementSetupComplete = true;
            panel.ConcreteCastingComplete = true;
            panel.SurfaceFinishingComplete = true;
            panel.CuringAndDemoldingComplete = true;
            panel.CurrentStage = PanelStageEnum.CuringAndDemolding;
            
            if (!panel.MoldPreparationDate.HasValue) panel.MoldPreparationDate = statusTime;
            if (!panel.InitialDate.HasValue) panel.InitialDate = statusTime;
            if (!panel.MEPInsertsInstallationDate.HasValue) panel.MEPInsertsInstallationDate = statusTime;
            if (!panel.ReinforcementSetupDate.HasValue) panel.ReinforcementSetupDate = statusTime;
            if (!panel.ConcreteCastingDate.HasValue) panel.ConcreteCastingDate = statusTime;
            if (!panel.SurfaceFinishingDate.HasValue) panel.SurfaceFinishingDate = statusTime;
            if (!panel.CuringAndDemoldingDate.HasValue) panel.CuringAndDemoldingDate = statusTime;

            // AUTO-UPDATE: Apply Mold Preparation completion to all panels with same type in project
            if (moldPrepWasIncompleteCompleted)
            {
                await ApplyMoldPreparationToSameTypePanels(panel, statusTime, currentUserId, cancellationToken);
            }

            panel.PanelStatus = PanelStatusEnum.Completed;
            panel.FirstApprovalStatus = "Pending";
        }
        // If status is PutOnHold, create quality issue only when panel is not already on hold
        else if (request.WorkflowStatus == "PutOnHold")
        {
            panel.PanelStatus = PanelStatusEnum.OnHold;
            
            // Only create new quality issue if panel doesn't already have one linked
            if (!panel.QualityIssueId.HasValue)
            {
                // Resolve QC Team for assignment (use requested team or find default QC Team)
                var assignedToTeamId = request.AssignedToTeamId;
                if (!assignedToTeamId.HasValue)
                {
                    var qcTeam = await _dbContext.Teams
                        .Where(t => t.IsActive && (
                            (t.TeamCode != null && (t.TeamCode.ToLower() == "qc" || t.TeamCode.ToLower() == "qc-team")) ||
                            (t.TeamName != null && (t.TeamName.ToLower().Contains("qc") || t.TeamName.ToLower().Contains("quality control")))))
                        .OrderBy(t => t.TeamCode ?? "")
                        .FirstOrDefaultAsync(cancellationToken);
                    assignedToTeamId = qcTeam?.TeamId;
                }

                // Create quality issue automatically for on hold panel
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId, cancellationToken);
                var reportedBy = user?.FullName ?? "System";
                
                // Generate issue number
                var issueCountInProject = _unitOfWork.Repository<QualityIssue>()
                    .GetWithSpec(new GetQualityIssuesSpecification()).Data
                    .Count(qi => qi.Box.ProjectId == panel.ProjectId || qi.ProjectId == panel.ProjectId);
                var issueNumber = (issueCountInProject + 1).ToString("D5");
                
                var description = $"Panel '{panel.PanelName}' put on hold at site (Pre-cast Location). {request.Notes}";
                
                var newIssue = new QualityIssue
                {
                    IssueNumber = issueNumber,
                    BoxId = panel.BoxId,
                    IssueType = IssueTypeEnum.Observation,
                    Severity = SeverityEnum.Minor,
                    IssueDescription = description,
                    NCR = NCRTypeEnum.Internal,
                    Status = QualityIssueStatusEnum.Open,
                    IssueDate = statusTime,
                    ReportedBy = reportedBy,
                    CreatedBy = currentUserId,
                    AssignedToTeamId = assignedToTeamId, // Assign to QC Team by default
                    CreatedDate = statusTime
                };
                
                await _unitOfWork.Repository<QualityIssue>().AddAsync(newIssue, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken); // Save to get IssueId
                
                // Link issue to panel
                panel.QualityIssueId = newIssue.IssueId;
            }
            // Panel already on hold with linked issue - just update workflow metadata (notes, date, etc.)
        }
        // Update PanelStatus enum based on workflow status
        else
        {
            panel.PanelStatus = request.WorkflowStatus switch
            {
                "InProgress" => PanelStatusEnum.InProgress,
                _ => panel.PanelStatus
            };
        }

        panel.ModifiedDate = statusTime;
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
            WorkflowStatus = panel.WorkflowStatus,
            CurrentStage = (int)panel.CurrentStage,
            MoldPreparationComplete = panel.MoldPreparationComplete,
            InitialComplete = panel.InitialComplete,
            MEPInsertsInstallationComplete = panel.MEPInsertsInstallationComplete,
            ReinforcementSetupComplete = panel.ReinforcementSetupComplete,
            ConcreteCastingComplete = panel.ConcreteCastingComplete,
            SurfaceFinishingComplete = panel.SurfaceFinishingComplete,
            CuringAndDemoldingComplete = panel.CuringAndDemoldingComplete,
            QualityIssueId = panel.QualityIssueId,
            CreatedDate = panel.CreatedDate,
            ModifiedDate = panel.ModifiedDate
        };

        return Result.Success(dto);
    }


    /// <summary>
    /// Helper method to automatically apply Mold Preparation completion to all panels 
    /// with the same type in the project
    /// </summary>
    private async Task ApplyMoldPreparationToSameTypePanels(
        BoxPanel currentPanel,
        DateTime statusTime,
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        if (!currentPanel.PanelTypeId.HasValue)
            return;

        var otherPanelsWithSameType = await _dbContext.BoxPanels
            .Where(p => p.ProjectId == currentPanel.ProjectId
                && p.PanelTypeId == currentPanel.PanelTypeId
                && p.BoxPanelId != currentPanel.BoxPanelId
                && !p.MoldPreparationComplete)
            .ToListAsync(cancellationToken);

        if (otherPanelsWithSameType.Any())
        {
            foreach (var otherPanel in otherPanelsWithSameType)
            {
                otherPanel.MoldPreparationComplete = true;
                otherPanel.MoldPreparationDate = statusTime;
                otherPanel.ModifiedDate = statusTime;
                otherPanel.ModifiedBy = currentUserId;

                // If panel was NotStarted, move to InProgress
                if (otherPanel.PanelStatus == PanelStatusEnum.NotStarted)
                {
                    otherPanel.PanelStatus = PanelStatusEnum.InProgress;
                    otherPanel.WorkflowStatus = "InProgress";
                }

                // Update current stage if not set or still at NotStarted
                if (otherPanel.CurrentStage == PanelStageEnum.NotStarted)
                {
                    otherPanel.CurrentStage = PanelStageEnum.MoldPreparation;
                }

                _unitOfWork.Repository<BoxPanel>().Update(otherPanel);
            }
        }
    }
}


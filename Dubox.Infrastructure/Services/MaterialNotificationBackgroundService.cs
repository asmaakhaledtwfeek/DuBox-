using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dubox.Infrastructure.Services;

/// <summary>
/// Background service that runs daily to check for materials that need attention
/// </summary>
public class MaterialNotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MaterialNotificationBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Run once daily

    public MaterialNotificationBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<MaterialNotificationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Material Notification Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessMaterialNotificationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing material notifications");
            }

            // Wait for next execution (24 hours)
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task ProcessMaterialNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        _logger.LogInformation("Checking materials for notifications...");

        var today = DateTime.UtcNow.Date;
        
        // Get all box materials that need attention
        var boxMaterials = await unitOfWork.Repository<BoxMaterial>()
            .FindAsync(bm => !bm.IsArrived, cancellationToken);

        var materialsToNotify = boxMaterials
            .Where(bm => 
            {
                var daysUntilRequired = (bm.RequiredByDate.Date - today).Days;
                // Send notification if: 10 days before, or already overdue
                return daysUntilRequired <= 10;
            })
            .ToList();

        _logger.LogInformation($"Found {materialsToNotify.Count} materials requiring attention");

        foreach (var boxMaterial in materialsToNotify)
        {
            await ProcessBoxMaterialNotificationAsync(boxMaterial, unitOfWork, cancellationToken);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        
        _logger.LogInformation("Material notification processing completed");
    }

    private async Task ProcessBoxMaterialNotificationAsync(
      BoxMaterial boxMaterial,
      IUnitOfWork unitOfWork,
      CancellationToken cancellationToken)
    {
        // ===== Base Guards =====
        if (boxMaterial.IsArrived)
            return;

        if (boxMaterial.NotificationsSentCount >= 2)
            return;

        // Load Box
        var box = await unitOfWork.Repository<Box>()
            .GetByIdAsync(boxMaterial.BoxId, cancellationToken);

        if (box == null)
            return;

        // Load Project
        var project = await unitOfWork.Repository<Project>()
            .GetByIdAsync(box.ProjectId, cancellationToken);

        if (project == null)
            return;

        // ===== Resolve Planned Start Date =====
        DateTime? plannedStartDate =
            box.PlannedStartDate ?? project.PlannedStartDate;

        if (!plannedStartDate.HasValue)
            return;

        // Load Material
        var material = await unitOfWork.Repository<Material>()
            .GetByIdAsync(boxMaterial.MaterialId, cancellationToken);

        if (material == null)
            return;

        var today = DateTime.UtcNow.Date;
        var daysUntilPlannedStart =
            (plannedStartDate.Value.Date - today).Days;

        var daysUntilRequired =
            (boxMaterial.RequiredByDate.Date - today).Days;

        bool sendAlert = false;
        bool sendDelayWarning = false;

        // ===== REQUIRED DATE LIMIT (2 TIMES ONLY) =====

        // First notification → before required date
        if (boxMaterial.NotificationsSentCount == 0 &&
            daysUntilRequired > 0)
        {
                if (daysUntilPlannedStart <=( boxMaterial.RequiredBeforeDays+10))
                    sendAlert = true;
        }

        // Second notification → at required date
        else if (boxMaterial.NotificationsSentCount == 1 &&
                 daysUntilRequired == 0)
        {
            sendDelayWarning = true;
        }

        if (!sendAlert && !sendDelayWarning)
            return;

        // ===== Prevent duplicate same-day =====
        if (boxMaterial.LastNotificationDate?.Date == today)
            return;

        // ===== Create Notification =====
        string title;
        string message;
        string priority;
        string notificationType;

        if (sendAlert)
        {
            title = $"Upcoming Material Requirement for Box {box.BoxTag}";
            message =
                $"Material '{material.MaterialName}' is required on {boxMaterial.RequiredByDate:yyyy-MM-dd}. " +
                $"Planned start date is in {daysUntilPlannedStart} days and material has not arrived yet.";
            priority = "Medium";
            notificationType = "Alert";
        }
        else
        {
            title = $"Box {box.BoxTag} Will Be Delayed";
            message =
                $"Material '{material.MaterialName}' was required today ({boxMaterial.RequiredByDate:yyyy-MM-dd}) " +
                $"and has not arrived yet. This will delay the box start.";
            priority = "High";
            notificationType = "Warning";
        }

        var notification = new Notification
        {
            NotificationType = notificationType,
            Priority = priority,
            Title = title,
            Message = message,
            RelatedBoxId = box.BoxId,
            TargetRole = "Project Manager",
            RecipientUserId = project.ProjectMangerId ?? project.CreatedBy,
            IsRead = false,
            CreatedDate = DateTime.UtcNow
        };

        await unitOfWork.Repository<Notification>()
            .AddAsync(notification, cancellationToken);

        // ===== Update tracking =====
        boxMaterial.LastNotificationDate = DateTime.UtcNow;
        boxMaterial.NotificationsSentCount++;
        unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);

        _logger.LogInformation(
            $"Notification sent | Box: {box.BoxTag} | Material: {material.MaterialName} | " +
            $"Count: {boxMaterial.NotificationsSentCount}");
    }


    private async Task CreateQualityIssueForDelayedMaterialAsync(
        BoxMaterial boxMaterial,
        Box box,
        Project project,
        Material material,
        int daysOverdue,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        // Generate issue number
        var existingIssues = await unitOfWork.Repository<QualityIssue>()
            .FindAsync(qi => qi.Box.ProjectId == project.ProjectId, cancellationToken);
        var issueNumber = (existingIssues.Count() + 1).ToString("D5");

        var qualityIssue = new QualityIssue
        {
            BoxId = box.BoxId,
            IssueNumber = issueNumber,
            IssueDate = DateTime.UtcNow,
            IssueType = IssueTypeEnum.NonConformance,
            Severity = SeverityEnum.Critical,
            IssueDescription = $"Material '{material.MaterialName}' has not arrived for Box {box.BoxTag}. " +
                              $"Required by: {boxMaterial.RequiredByDate:yyyy-MM-dd}. " +
                              $"Currently overdue by {daysOverdue} days. " +
                              $"This delay will impact the production schedule and may cause delays to downstream activities.",
            ReportedBy = "System - Material Tracking",
            Status = QualityIssueStatusEnum.Open,
            NCR = NCRTypeEnum.Internal,
            CreatedDate = DateTime.UtcNow
        };

        // Quality issue will be visible to project team members
        // Specific assignment can be done manually through the UI

        await unitOfWork.Repository<QualityIssue>().AddAsync(qualityIssue, cancellationToken);
        
        // Link quality issue to box material
        boxMaterial.QualityIssueId = qualityIssue.IssueId;
        unitOfWork.Repository<BoxMaterial>().Update(boxMaterial);

        _logger.LogWarning(
            $"Quality issue {issueNumber} created for delayed material {material.MaterialName} (Box: {box.BoxTag})");
    }
}


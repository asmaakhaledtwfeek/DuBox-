using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using System.Linq;

namespace Dubox.Application.Services;

public interface IProjectProgressService
{
    Task<(decimal? oldProgress, decimal newProgress)> UpdateProjectProgressAsync(Guid projectId, Guid changedBy, string reason, CancellationToken cancellationToken);
}

public class ProjectProgressService : IProjectProgressService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectProgressService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(decimal? oldProgress, decimal newProgress)> UpdateProjectProgressAsync(Guid projectId, Guid changedBy, string reason, CancellationToken cancellationToken)
    {
        var projectRepository = _unitOfWork.Repository<Project>();
        var project = await projectRepository.GetByIdAsync(projectId, cancellationToken);

        if (project == null)
            throw new InvalidOperationException("Project not found while updating progress.");

        var boxes = await _unitOfWork.Repository<Box>()
            .FindAsync(b => b.ProjectId == projectId, cancellationToken);

        decimal newProgress = 0;
        if (boxes.Any())
        {
            var averageProgress = boxes.Average(b => b.ProgressPercentage);
            newProgress = Math.Round(averageProgress, 2);
        }

        var oldProgress = project.ProgressPercentage;

        if (oldProgress == newProgress)
        {
            return (oldProgress, newProgress);
        }

        project.ProgressPercentage = newProgress;
        projectRepository.Update(project);

        var auditLog = new AuditLog
        {
            TableName = nameof(Project),
            RecordId = project.ProjectId,
            Action = "ProgressUpdate",
            OldValues = $"ProgressPercentage: {oldProgress}",
            NewValues = $"ProgressPercentage: {newProgress}",
            ChangedBy = changedBy,
            ChangedDate = DateTime.UtcNow,
            Description = reason
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return (oldProgress, newProgress);
    }
}


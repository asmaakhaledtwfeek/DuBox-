using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UpdateCompressionStartDateCommandHandler : IRequestHandler<UpdateCompressionStartDateCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCompressionStartDateCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateCompressionStartDateCommand request, CancellationToken cancellationToken)
    {
        var projectRepository = _unitOfWork.Repository<Project>();
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectDto>("Project not found.");
        }

        var oldCompressionStartDate = project.CompressionStartDate;
        project.CompressionStartDate = request.CompressionStartDate;
        project.ModifiedDate = DateTime.UtcNow;
        projectRepository.Update(project);

        var changedBy = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var auditLog = new AuditLog
        {
            TableName = nameof(Project),
            RecordId = project.ProjectId,
            Action = "CompressionStartDateUpdate",
            OldValues = oldCompressionStartDate.HasValue 
                ? $"CompressionStartDate: {oldCompressionStartDate.Value:yyyy-MM-dd}" 
                : "CompressionStartDate: null",
            NewValues = request.CompressionStartDate.HasValue 
                ? $"CompressionStartDate: {request.CompressionStartDate.Value:yyyy-MM-dd}" 
                : "CompressionStartDate: null",
            ChangedBy = changedBy,
            ChangedDate = DateTime.UtcNow,
            Description = request.CompressionStartDate.HasValue
                ? $"Project compression start date set to {request.CompressionStartDate.Value:yyyy-MM-dd}."
                : "Project compression start date cleared."
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }
}


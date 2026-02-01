using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class UploadProjectImagesCommandHandler : IRequestHandler<UploadProjectImagesCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private const string ContainerName = "images";

    public UploadProjectImagesCommandHandler(
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService,
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProjectDto>> Handle(UploadProjectImagesCommand request, CancellationToken cancellationToken)
    {
        // Check permissions
        var module = PermissionModuleEnum.Projects;
        var action = PermissionActionEnum.Edit;
        var canModify = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if (!canModify)
            return Result.Failure<ProjectDto>("Access denied. You do not have permission to upload project images.");

        // Get project
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectDto>("Project not found.");

        // Verify user has access to the project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccessProject)
            return Result.Failure<ProjectDto>("Access denied. You do not have permission to update this project.");

        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(request.ProjectId, "upload project images", cancellationToken);
        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<ProjectDto>(projectStatusValidation.Error!);

        try
        {
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var changes = new List<string>();

            // Upload Contractor Image
            if (request.ContractorImage != null)
            {
                var contractorImageUrl = await _blobStorageService.UploadFileAsync(
                    ContainerName,
                    request.ContractorImage,
                    $"{request.ProjectId}/contractor");

                // Delete old image if exists
                if (!string.IsNullOrEmpty(project.ContractorImageUrl))
                {
                    var oldFileName = Path.GetFileName(project.ContractorImageUrl);
                    await _blobStorageService.DeleteFileAsync(ContainerName, $"{request.ProjectId}/contractor/{oldFileName}");
                }

                project.ContractorImageUrl = contractorImageUrl;
                changes.Add("Contractor Image");
            }

            // Upload Sub-contractor Image
            if (request.SubContractorImage != null)
            {
                var subContractorImageUrl = await _blobStorageService.UploadFileAsync(
                    ContainerName,
                    request.SubContractorImage,
                    $"{request.ProjectId}/subcontractor");

                // Delete old image if exists
                if (!string.IsNullOrEmpty(project.SubContractorImageUrl))
                {
                    var oldFileName = Path.GetFileName(project.SubContractorImageUrl);
                    await _blobStorageService.DeleteFileAsync(ContainerName, $"{request.ProjectId}/subcontractor/{oldFileName}");
                }

                project.SubContractorImageUrl = subContractorImageUrl;
                changes.Add("Sub-contractor Image");
            }

            // Upload Client Image
            if (request.ClientImage != null)
            {
                var clientImageUrl = await _blobStorageService.UploadFileAsync(
                    ContainerName,
                    request.ClientImage,
                    $"{request.ProjectId}/client");

                // Delete old image if exists
                if (!string.IsNullOrEmpty(project.ClientImageUrl))
                {
                    var oldFileName = Path.GetFileName(project.ClientImageUrl);
                    await _blobStorageService.DeleteFileAsync(ContainerName, $"{request.ProjectId}/client/{oldFileName}");
                }

                project.ClientImageUrl = clientImageUrl;
                changes.Add("Client Image");
            }

            if (changes.Count == 0)
                return Result.Failure<ProjectDto>("No images provided to upload.");

            // Update project
            project.ModifiedDate = DateTime.UtcNow;
            project.ModifiedBy = currentUserId.ToString();

            _unitOfWork.Repository<Project>().Update(project);

            // Create audit log
            var auditLog = new AuditLog
            {
                TableName = nameof(Project),
                RecordId = project.ProjectId,
                Action = "Update",
                OldValues = "Previous images",
                NewValues = string.Join(", ", changes),
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Project '{project.ProjectName}' images updated: {string.Join(", ", changes)}"
            };

            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(project.Adapt<ProjectDto>());
        }
        catch (Exception ex)
        {
            return Result.Failure<ProjectDto>($"Failed to upload project images: {ex.Message}");
        }
    }
}

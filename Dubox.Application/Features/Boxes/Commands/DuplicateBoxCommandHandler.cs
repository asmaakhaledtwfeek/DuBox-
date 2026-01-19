using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Boxes.Commands;

public class DuplicateBoxCommandHandler : IRequestHandler<DuplicateBoxCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISerialNumberService _serialNumberService;
    private readonly IQRCodeService _qrCodeService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IBoxActivityService _boxActivityService;
    private readonly IProjectProgressService _projectProgressService;
    private readonly ILogger<DuplicateBoxCommandHandler> _logger;
    private readonly IBoxCreationService _boxCreationService;
    public DuplicateBoxCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        ISerialNumberService serialNumberService,
        IQRCodeService qrCodeService,
        IProjectTeamVisibilityService visibilityService,
        IBoxActivityService boxActivityService,
        IProjectProgressService projectProgressService,
        ILogger<DuplicateBoxCommandHandler> logger,
        IBoxCreationService boxCreationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _serialNumberService = serialNumberService;
        _qrCodeService = qrCodeService;
        _visibilityService = visibilityService;
        _boxActivityService = boxActivityService;
        _projectProgressService = projectProgressService;
        _logger = logger;
        _boxCreationService = boxCreationService;

    }

    public async Task<Result<BoxDto>> Handle(DuplicateBoxCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Duplicating box {BoxId}", request.BoxId);

        // Check permissions
        var module = PermissionModuleEnum.Boxes;
        var action = PermissionActionEnum.Create;
        var canCreate = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if (!canCreate)
            return Result.Failure<BoxDto>("Access denied. You do not have permission to create boxes.");

        var originalBox = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (originalBox == null)
            return Result.Failure<BoxDto>("Box not found");

        // Get project
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(originalBox.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<BoxDto>("Project not found");

        // Verify user has access to the project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(originalBox.ProjectId, cancellationToken);
        if (!canAccessProject)
            return Result.Failure<BoxDto>("Access denied. You do not have permission to duplicate boxes for this project.");

        // Check project status
        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(
            originalBox.ProjectId, "duplicate boxes", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<BoxDto>(projectStatusValidation.Error!);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        Box newBox = originalBox;
        newBox.BoxId = Guid.Empty;
        newBox.ProgressPercentage = 0;
        newBox.Status = BoxStatusEnum.NotStarted;
        newBox.ActualStartDate = null;
        newBox.ActualEndDate = null;
        newBox.Row = null;
        newBox.Bay = null;
        newBox.Position = null;
        newBox.ModifiedBy = null;
        newBox.ModifiedDate = null;
        var boxDto= await _boxCreationService.CreateAsync(newBox, project, currentUserId, "Dublication", $"New Box '{originalBox.BoxTag}' duplicated successfully under Project '{project.ProjectCode}'.", cancellationToken);

        return Result.Success(boxDto);
    }

}


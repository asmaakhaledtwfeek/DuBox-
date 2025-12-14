using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class CreateBoxCommandHandler : IRequestHandler<CreateBoxCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IQRCodeService _qrCodeService;
    private readonly IBoxActivityService _boxActivityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectProgressService _projectProgressService;
    private readonly ISerialNumberService _serialNumberService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public CreateBoxCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        IMapper Mapper,
        IQRCodeService qrCodeService,
        IBoxActivityService boxActivityService,
        ICurrentUserService currentUserService,
        IProjectProgressService projectProgressService,
        ISerialNumberService serialNumberService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _mapper = Mapper;
        _qrCodeService = qrCodeService;
        _boxActivityService = boxActivityService;
        _currentUserService = currentUserService;
        _projectProgressService = projectProgressService;
        _serialNumberService = serialNumberService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxDto>> Handle(CreateBoxCommand request, CancellationToken cancellationToken)
    {
        // Check if user can modify data (Viewer role cannot)
        var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
        if (!canModify)
        {
            return Result.Failure<BoxDto>("Access denied. Viewer role has read-only access and cannot create boxes.");
        }

        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<BoxDto>("Project not found");

        // Verify user has access to the project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccessProject)
        {
            return Result.Failure<BoxDto>("Access denied. You do not have permission to create boxes for this project.");
        }

        var boxExists = await _unitOfWork.Repository<Box>()
            .IsExistAsync(b => b.ProjectId == request.ProjectId && b.BoxTag == request.BoxTag, cancellationToken);

        if (boxExists)
            return Result.Failure<BoxDto>("Box with this tag already exists in the project");

        var box = _mapper.Map<Box>(request);

        if (request.BoxPlannedStartDate.HasValue && request.BoxDuration.HasValue)
            box.PlannedEndDate = request.BoxPlannedStartDate.Value.AddDays(request.BoxDuration.Value);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        // Generate unique serial number
        box.SerialNumber = _serialNumberService.GenerateSerialNumber();
        
        // Generate QR code string with structured format
        box.QRCodeString = $"ProjectCode: {project.ProjectCode}\nBoxTag: {request.BoxTag}\nSerialNumber: {box.SerialNumber}";
        box.CreatedBy = currentUserId;
        box.BoxAssets = request.Assets?.Adapt<List<BoxAsset>>() ?? new List<BoxAsset>();
        foreach (var asset in box.BoxAssets)
            asset.Box = box;

        await _unitOfWork.Repository<Box>().AddAsync(box, cancellationToken);

        await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);
        var oldTotalBoxes = project.TotalBoxes;

        project.TotalBoxes++;
        _unitOfWork.Repository<Project>().Update(project);

        const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        var boxLog = new AuditLog
        {
            TableName = nameof(Box),
            RecordId = box.BoxId,
            Action = "Creation",
            OldValues = "N/A (New Entity)",
            NewValues = $"Tag: {box.BoxTag}, ProjectId: {box.ProjectId}, PlannedStart: {box.PlannedStartDate?.ToString(dateFormat) ?? "N/A"}, Duration: {box.Duration}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New Box '{box.BoxTag}' created successfully under Project '{project.ProjectCode}'."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(boxLog, cancellationToken);
        var projectLog = new AuditLog
        {
            TableName = nameof(Project),
            RecordId = project.ProjectId,
            Action = "TotalBoxesUpdate",
            OldValues = $"TotalBoxes: {oldTotalBoxes}",
            NewValues = $"TotalBoxes: {project.TotalBoxes}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"Total box count incremented from {oldTotalBoxes} to {project.TotalBoxes} due to new box creation."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        await _projectProgressService.UpdateProjectProgressAsync(
            project.ProjectId,
            currentUserId,
            $"Project progress recalculated due to new box '{box.BoxTag}' creation.",
            cancellationToken);

        var boxDto = box.Adapt<BoxDto>() with
        { QRCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString) };

        return Result.Success(boxDto);

    }

}


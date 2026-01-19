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
using System.Diagnostics;

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
    private readonly IBoxCreationService _boxCreationService;


    public CreateBoxCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        IMapper Mapper,
        IQRCodeService qrCodeService,
        IBoxActivityService boxActivityService,
        ICurrentUserService currentUserService,
        IProjectProgressService projectProgressService,
        ISerialNumberService serialNumberService,
        IProjectTeamVisibilityService visibilityService,
        IBoxCreationService boxCreationService)
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
        _boxCreationService = boxCreationService;
    }

    public async Task<Result<BoxDto>> Handle(CreateBoxCommand request, CancellationToken cancellationToken)
    {
        var module= PermissionModuleEnum.Boxes;
        var action = PermissionActionEnum.Create;
        var canCreate = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if (!canCreate)
            return Result.Failure<BoxDto>("Access denied. You do not have permission to create boxes.");

        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<BoxDto>("Project not found");

        // Verify user has access to the project
        var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
        if (!canAccessProject)
            return Result.Failure<BoxDto>("Access denied. You do not have permission to create boxes for this project.");
        
        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(request.ProjectId, "create boxes", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<BoxDto>(projectStatusValidation.Error!);
        

        var box = _mapper.Map<Box>(request);

        if (request.BoxPlannedStartDate.HasValue && request.BoxDuration.HasValue)
            box.PlannedEndDate = request.BoxPlannedStartDate.Value.AddDays(request.BoxDuration.Value);

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
       
        var factory = _unitOfWork.Repository<Factory>()
            .Get()
            .Where(f => f.Location == project.Location && f.IsActive)
            .OrderBy(f => f.CurrentOccupancy) // Prefer factories with lower occupancy
            .FirstOrDefault();
        
        if (factory != null)
            box.FactoryId = factory.FactoryId;
       
        
        box.BoxAssets = request.Assets?.Adapt<List<BoxAsset>>() ?? new List<BoxAsset>();
        foreach (var asset in box.BoxAssets)
            asset.Box = box;
       var boxDto= await _boxCreationService.CreateAsync(box, project, currentUserId, "Creation", $"New Box '{box.BoxTag}' created successfully under Project '{project.ProjectCode}'.", cancellationToken);

        return Result.Success(boxDto);

    }

}


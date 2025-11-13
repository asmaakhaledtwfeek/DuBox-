using Dubox.Application.DTOs;
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

    public CreateBoxCommandHandler(IUnitOfWork unitOfWork, IDbContext dbContext, IMapper Mapper, IQRCodeService qrCodeService, IBoxActivityService boxActivityService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _mapper = Mapper;
        _qrCodeService = qrCodeService;
        _boxActivityService = boxActivityService;
    }

    public async Task<Result<BoxDto>> Handle(CreateBoxCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<BoxDto>("Project not found");

        var boxExists = await _unitOfWork.Repository<Box>()
            .IsExistAsync(b => b.ProjectId == request.ProjectId && b.BoxTag == request.BoxTag, cancellationToken);

        if (boxExists)
            return Result.Failure<BoxDto>("Box with this tag already exists in the project");

        var box = _mapper.Map<Box>(request);

        box.QRCodeString = $"{project.ProjectCode}_{request.BoxTag}";
        if (request.Assets != null && request.Assets.Any())
        {
            var boxAssets = request.Assets.Adapt<List<BoxAsset>>();
            box.BoxAssets = boxAssets;
        }
        else
            box.BoxAssets = new List<BoxAsset>();
        foreach (var asset in box.BoxAssets)
            asset.Box = box;

        await _unitOfWork.Repository<Box>().AddAsync(box, cancellationToken);

        await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        project.TotalBoxes++;
        _unitOfWork.Repository<Project>().Update(project);
        await _unitOfWork.CompleteAsync(cancellationToken);
        var boxDto = box.Adapt<BoxDto>() with { QRCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString) };

        return Result.Success(boxDto);

    }

}


using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxByIdQueryHandler : IRequestHandler<GetBoxByIdQuery, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IQRCodeService qRCodeService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qRCodeService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxDto>> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
    {
        var box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxByIdWithIncludesSpecification(request.BoxId));

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
        if (!canAccessProject)
        {
            return Result.Failure<BoxDto>("Access denied. You do not have permission to view this box.");
        }

        var boxDto = box.Adapt<BoxDto>() with
        {
            ProjectCode = box.Project.ProjectCode,
            QRCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString),
            CurrentLocationId = box.CurrentLocationId,
            CurrentLocationCode = box.CurrentLocation?.LocationCode,
            CurrentLocationName = box.CurrentLocation?.LocationName
        };

        return Result.Success(boxDto);
    }
}


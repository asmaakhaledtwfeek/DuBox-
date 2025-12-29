using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxByIdQueryHandler : IRequestHandler<GetBoxByIdQuery, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IBoxMapper _boxMapper;

    public GetBoxByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IQRCodeService qRCodeService,
        IProjectTeamVisibilityService visibilityService , IBoxMapper boxMapper)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qRCodeService;
        _visibilityService = visibilityService;
        _boxMapper = boxMapper;
    }

    public async Task<Result<BoxDto>> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxByIdWithIncludesSpecification(request.BoxId));

            if (box == null)
                return Result.Failure<BoxDto>("Box not found");

            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<BoxDto>("Access denied. You do not have permission to view this box.");
            }

            var boxDto = _boxMapper.Map(box);

            return Result.Success(boxDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxDto>($"Error in GetBoxByIdQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
        }
    }

   
}


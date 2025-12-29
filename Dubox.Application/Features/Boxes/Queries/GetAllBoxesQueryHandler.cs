using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetAllBoxesQueryHandler : IRequestHandler<GetAllBoxesQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IBoxMapper _boxMapper;

    public GetAllBoxesQueryHandler(
        IUnitOfWork unitOfWork, 
        IQRCodeService qRCodeService,
        IProjectTeamVisibilityService visibilityService , IBoxMapper boxMapper)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qRCodeService;
        _visibilityService = visibilityService;
        _boxMapper = boxMapper;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetAllBoxesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get accessible project IDs based on user role
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            
            // Use AsNoTracking and ToListAsync for better performance
            var boxes = await _unitOfWork.Repository<Box>()
                .GetWithSpec(new GetAllBoxesWithIncludesSpecification(accessibleProjectIds)).Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var boxDtos = new List<BoxDto>();

            foreach (var box in boxes)
            {
                try
                {
                    var dto = _boxMapper.Map(box);
                    boxDtos.Add(dto);
                }
                catch (Exception ex)
                {
                    // Log the error for this specific box but continue processing others
                    Console.WriteLine($"Warning: Error mapping box {box.BoxId}: {ex.Message}");
                    // Optionally, you could skip this box or add a partial DTO
                    // For now, we'll skip boxes that fail to map
                }
            }

            return Result.Success(boxDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<BoxDto>>($"Error in GetAllBoxesQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
        }
    }

   
}


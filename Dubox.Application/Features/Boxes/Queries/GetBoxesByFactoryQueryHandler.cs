using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxesByFactoryQueryHandler : IRequestHandler<GetBoxesByFactoryQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IBoxMapper _boxMapper;
    public GetBoxesByFactoryQueryHandler(
        IUnitOfWork unitOfWork,
        IQRCodeService qrCodeService , IBoxMapper boxMapper)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qrCodeService;
        _boxMapper = boxMapper;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetBoxesByFactoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify factory exists
            var factory = await _unitOfWork.Repository<Factory>().GetByIdAsync(request.FactoryId, cancellationToken);
            if (factory == null)
            {
                return Result.Failure<List<BoxDto>>("Factory not found");
            }

            // Get boxes for this factory (InProgress, Completed, and Dispatched)
            var boxes =  _unitOfWork.Repository<Box>()
                .GetWithSpec(new GetBoxesByFactoryIdSpecification(request.FactoryId)).Data.ToList();

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
                }
            }

            return Result.Success(boxDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<BoxDto>>($"Error in GetBoxesByFactoryQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
        }
    }

}


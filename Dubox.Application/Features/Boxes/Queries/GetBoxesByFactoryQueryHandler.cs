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

public class GetBoxesByFactoryQueryHandler : IRequestHandler<GetBoxesByFactoryQuery, Result<PaginatedBoxesByFactoryResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IBoxMapper _boxMapper;
    public GetBoxesByFactoryQueryHandler(
        IUnitOfWork unitOfWork,
        IQRCodeService qrCodeService, IBoxMapper boxMapper)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qrCodeService;
        _boxMapper = boxMapper;
    }

    public async Task<Result<PaginatedBoxesByFactoryResponseDto>> Handle(GetBoxesByFactoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify factory exists
            var factory = await _unitOfWork.Repository<Factory>().GetByIdAsync(request.FactoryId, cancellationToken);
            if (factory == null)
            {
                return Result.Failure<PaginatedBoxesByFactoryResponseDto>("Factory not found");
            }

            // Normalize pagination parameters
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 ? 25 : (request.PageSize > 100 ? 100 : request.PageSize);

            // Get boxes for this factory (InProgress, Completed, and optionally Dispatched)
            var specification = new GetBoxesByFactoryIdSpecification(request.FactoryId, request.IncludeDispatched);
            var boxesResult = _unitOfWork.Repository<Box>().GetWithSpec(specification);
            
            // Get total count before pagination
            var totalCount = boxesResult.Count;
            
            // Apply pagination
            var boxes = boxesResult.Data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedBoxesByFactoryResponseDto
            {
                Items = boxDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedBoxesByFactoryResponseDto>($"Error in GetBoxesByFactoryQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
        }
    }

}


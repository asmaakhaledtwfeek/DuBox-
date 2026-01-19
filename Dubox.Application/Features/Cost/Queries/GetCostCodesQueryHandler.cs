using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public class GetCostCodesQueryHandler : IRequestHandler<GetCostCodesQuery, Result<CostCodesResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCostCodesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CostCodesResponse>> Handle(GetCostCodesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Create specification with search filters
            var specification = new GetCostCodesSpecification(request);

            // Apply specification to get filtered and paginated results
            var (data, totalCount) = _unitOfWork.Repository<CostCodeMaster>().GetWithSpec(specification);

            // Map to DTOs
            var costCodes = data.Select(c => new CostCodeListDto(
                c.CostCodeId,
                c.Code,
                c.CostCodeLevel1,
                c.Level1Description,
                c.CostCodeLevel2,
                c.Level2Description,
                c.CostCodeLevel3,
                c.Description,
                c.Level3DescriptionAbbrev,
                c.Level3DescriptionAmana,
                c.Category,
                c.UnitOfMeasure,
                c.UnitRate,
                c.Currency,
                c.IsActive
            )).ToList();

            var response = new CostCodesResponse(costCodes, totalCount);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<CostCodesResponse>(new Error("QueryFailed", $"Failed to retrieve cost codes: {ex.Message}"));
        }
    }
}


using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public class GetHRCostsQueryHandler : IRequestHandler<GetHRCostsQuery, Result<HRCostsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHRCostsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<HRCostsResponse>> Handle(GetHRCostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Create specification with search filters
            var specification = new GetHRCostsSpecification(request);

            // Apply specification to get filtered and paginated results
            var (data, totalCount) = _unitOfWork.Repository<HRCostRecord>().GetWithSpec(specification);

            // Map to DTOs
            var hrCosts = data.Select(h => new HRCostDto(
                h.HRCostRecordId,
                h.Code,
                h.Chapter,
                h.SubChapter,
                h.Classification,
                h.SubClassification,
                h.Name,
                h.Units,
                h.Type,
                h.BudgetLevel,
                h.Status,
                h.Job,
                h.OfficeAccount,
                h.JobCostAccount,
                h.SpecialAccount,
                h.IDLAccount
            )).ToList();

            var response = new HRCostsResponse(hrCosts, totalCount);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<HRCostsResponse>(new Error("QueryFailed", $"Failed to retrieve HR costs: {ex.Message}"));
        }
    }
}


using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesByBoxIdQueryHandler : IRequestHandler<GetQualityIssuesByBoxIdQuery, Result<List<QualityIssueDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQualityIssuesByBoxIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<QualityIssueDetailsDto>>> Handle(GetQualityIssuesByBoxIdQuery request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
            if (box == null)
                return Result.Failure<List<QualityIssueDetailsDto>>("Box not found");

            var specificationResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesByBoxIdSpecification(request.BoxId));
            var issues = specificationResult.Data.ToList();

            var dtos = issues.Adapt<List<QualityIssueDetailsDto>>();

            return Result.Success(dtos);
        }
    }

}

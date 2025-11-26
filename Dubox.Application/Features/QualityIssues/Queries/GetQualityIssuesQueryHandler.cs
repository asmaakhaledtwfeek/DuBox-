using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesQueryHandler : IRequestHandler<GetQualityIssuesQuery, Result<List<QualityIssueDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQualityIssuesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<QualityIssueDetailsDto>>> Handle(GetQualityIssuesQuery request, CancellationToken cancellationToken)
        {
            var qualityIssues = _unitOfWork.Repository<QualityIssue>().GetWithSpec(new GetQualityIssuesSpecification(request)).Data.ToList();

            var dtos = qualityIssues.Adapt<List<QualityIssueDetailsDto>>();

            return Result.Success(dtos);
        }
    }

}

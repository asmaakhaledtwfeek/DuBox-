using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssueByIdQueryHandler : IRequestHandler<GetQualityIssueByIdQuery, Result<QualityIssueDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQualityIssueByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<QualityIssueDetailsDto>> Handle(GetQualityIssueByIdQuery request, CancellationToken cancellationToken)
        {
            var issue = _unitOfWork.Repository<QualityIssue>().GetEntityWithSpec(new GetQualityIssueByIdSpecification(request.IssueId));

            if (issue is null)
                return Result.Failure<QualityIssueDetailsDto>("Quality Issue not found.");

            return Result.Success(issue.Adapt<QualityIssueDetailsDto>());
        }
    }

}

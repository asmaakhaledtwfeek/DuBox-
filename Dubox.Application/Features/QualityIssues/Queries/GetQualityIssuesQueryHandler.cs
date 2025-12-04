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

            var dtos = qualityIssues.Select(issue =>
            {
                var dto = issue.Adapt<QualityIssueDetailsDto>();
                dto.Images = issue.Images
                    .OrderBy(img => img.Sequence)
                    .Select(img => new QualityIssueImageDto
                    {
                        QualityIssueImageId = img.QualityIssueImageId,
                        IssueId = img.IssueId,
                        ImageData = img.ImageData,
                        ImageType = img.ImageType,
                        OriginalName = img.OriginalName,
                        FileSize = img.FileSize,
                        Sequence = img.Sequence,
                        CreatedDate = img.CreatedDate
                    }).ToList();
                return dto;
            }).ToList();

            return Result.Success(dtos);
        }
    }

}

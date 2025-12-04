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

            var dtos = issues.Select(issue =>
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

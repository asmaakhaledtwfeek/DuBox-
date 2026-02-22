using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesByBoxIdQueryHandler : IRequestHandler<GetQualityIssuesByBoxIdQuery, Result<List<QualityIssueDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly IQualityIssueMappingService _mappingService;

        public GetQualityIssuesByBoxIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IProjectTeamVisibilityService visibilityService,
            IQualityIssueMappingService mappingService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
            _mappingService = mappingService;
        }

        public async Task<Result<List<QualityIssueDetailsDto>>> Handle(GetQualityIssuesByBoxIdQuery request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
            if (box == null)
                return Result.Failure<List<QualityIssueDetailsDto>>("Box not found");

            // Verify user has access to the project this box belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<List<QualityIssueDetailsDto>>("Access denied. You do not have permission to view quality issues for this box.");
            }

            var specificationResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesByBoxIdSpecification(request.BoxId));
            var issues = specificationResult.Data.ToList();

            // Use mapping service to convert issues to DTOs
            var dtos = _mappingService.MapToDtoList(issues);

            return Result.Success(dtos);
        }
    }

}

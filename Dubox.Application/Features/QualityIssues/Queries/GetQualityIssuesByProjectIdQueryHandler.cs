using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesByProjectIdQueryHandler : IRequestHandler<GetQualityIssuesByProjectIdQuery, Result<List<QualityIssueDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly IQualityIssueMappingService _mappingService;

        public GetQualityIssuesByProjectIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IProjectTeamVisibilityService visibilityService,
            IQualityIssueMappingService mappingService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
            _mappingService = mappingService;
        }

        public async Task<Result<List<QualityIssueDetailsDto>>> Handle(GetQualityIssuesByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
                return Result.Failure<List<QualityIssueDetailsDto>>("Project not found");

            // Verify user has access to the project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<List<QualityIssueDetailsDto>>("Access denied. You do not have permission to view quality issues for this project.");
            }

            // Optimized: Use AsNoTracking and async ToList for better performance
            var specificationResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesByProjectIdSpecification(request.ProjectId));
            
            var issues = await specificationResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Use mapping service to convert issues to DTOs
            var dtos = _mappingService.MapToDtoList(issues);

            return Result.Success(dtos);
        }
    }
}













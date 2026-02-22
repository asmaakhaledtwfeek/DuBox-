using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointFiltersQueryHandler : IRequestHandler<GetWIRCheckpointFiltersQuery, Result<WIRCheckpointFiltersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetWIRCheckpointFiltersQueryHandler(
            IUnitOfWork unitOfWork,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
        }

        public async Task<Result<WIRCheckpointFiltersDto>> Handle(GetWIRCheckpointFiltersQuery request, CancellationToken cancellationToken)
        {
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);

            // Get WIRCheckpoints using filter-specific specification (without pagination)
            var checkpointsQuery = await _unitOfWork.Repository<WIRCheckpoint>()
                .GetWithSpec(new GetWIRCheckpointFiltersSpecification(accessibleProjectIds))
                .Data
                .ToListAsync(cancellationToken);
                

            

            // Get distinct stage numbers (WIRCode)
            var stageNumbers =  checkpointsQuery
                .Select(w => w.WIRCode)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            // Get distinct box tags
            var boxTags =  checkpointsQuery
                .Select(w => w.Box.BoxTag)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            // Get distinct project codes
            var projectCodes =  checkpointsQuery
                .Where(w => w.Box.Project != null && !string.IsNullOrEmpty(w.Box.Project.ProjectCode))
                .Select(w => w.Box.Project.ProjectCode)
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            // Get box tags with their project codes for cascading filter
            var boxTagsWithProjects = checkpointsQuery
                .Where(w => w.Box.Project != null && !string.IsNullOrEmpty(w.Box.Project.ProjectCode))
                .Select(w => new BoxTagWithProject
                {
                    BoxTag = w.Box.BoxTag,
                    ProjectCode = w.Box.Project.ProjectCode
                })
                .Distinct()
                .OrderBy(b => b.ProjectCode)
                .ThenBy(b => b.BoxTag)
                .ToList();

            var result = new WIRCheckpointFiltersDto
            {
                StageNumbers = stageNumbers,
                BoxTags = boxTags,
                ProjectCodes = projectCodes,
                BoxTagsWithProjects = boxTagsWithProjects
            };

            return Result.Success(result);
        }
    }
}

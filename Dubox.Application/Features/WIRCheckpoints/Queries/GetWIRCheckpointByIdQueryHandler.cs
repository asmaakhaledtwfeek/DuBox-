using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointByIdQueryHandler
    : IRequestHandler<GetWIRCheckpointByIdQuery, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly ICurrentUserService _currentUserService;

        public GetWIRCheckpointByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(GetWIRCheckpointByIdQuery request, CancellationToken cancellationToken)
        {

            var checkpoint = _unitOfWork.Repository<WIRCheckpoint>().
                GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (checkpoint == null)
                return Result.Failure<WIRCheckpointDto>("WIR checkpoint not found");

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

            var canAccessProject = await _visibilityService.CanAccessProjectAsync(checkpoint.Box.ProjectId, cancellationToken);
            if (!canAccessProject && (checkpoint.InspectorId !=null && checkpoint.InspectorId != currentUserId))
                return Result.Failure<WIRCheckpointDto>("Access denied. You do not have permission to view this WIR checkpoint.");

            var dto = checkpoint.Adapt<WIRCheckpointDto>();

            if (dto.ChecklistItems != null && checkpoint.ChecklistItems != null)
            {
                foreach (var dtoItem in dto.ChecklistItems)
                {
                    // Find the corresponding entity item
                    var entityItem = checkpoint.ChecklistItems
                        .FirstOrDefault(ci => ci.ChecklistItemId == dtoItem.ChecklistItemId);
                    
                    if (entityItem?.PredefinedChecklistItem != null)
                    {
                        var predefinedItem = entityItem.PredefinedChecklistItem;
                        
                        // Map section information
                        if (predefinedItem.ChecklistSection != null)
                        {
                            dtoItem.SectionId = predefinedItem.ChecklistSection.ChecklistSectionId;
                            dtoItem.SectionName = predefinedItem.ChecklistSection.Title;
                            dtoItem.SectionOrder = predefinedItem.ChecklistSection.Order;
                            
                            // Map checklist information
                            if (predefinedItem.ChecklistSection.Checklist != null)
                            {
                                dtoItem.ChecklistId = predefinedItem.ChecklistSection.Checklist.ChecklistId;
                                dtoItem.ChecklistName = predefinedItem.ChecklistSection.Checklist.Name;
                                dtoItem.ChecklistCode = predefinedItem.ChecklistSection.Checklist.Code;
                                
                                // Use checklist name as category if not already set
                                if (string.IsNullOrEmpty(dtoItem.CategoryName))
                                {
                                    dtoItem.CategoryName = predefinedItem.ChecklistSection.Checklist.Name;
                                }
                            }
                        }
                    }
                }
            }

            // Map quality issues navigation properties (CCUser and AssignedToMember)
            if (dto.QualityIssues != null && checkpoint.QualityIssues != null)
            {
                foreach (var dtoIssue in dto.QualityIssues)
                {
                    // Find the corresponding entity issue
                    var entityIssue = checkpoint.QualityIssues
                        .FirstOrDefault(qi => qi.IssueId == dtoIssue.IssueId);
                    
                    if (entityIssue != null)
                    {
                        // Map CC User name
                        if (entityIssue.CCUser != null)
                        {
                            dtoIssue.CcUserName = entityIssue.CCUser.FullName;
                        }
                        
                        // Map Assigned To Member name
                        if (entityIssue.AssignedToMember != null)
                        {
                            dtoIssue.AssignedUserName =!string.IsNullOrEmpty( entityIssue.AssignedToMember.EmployeeName) ? entityIssue.AssignedToMember.EmployeeName : entityIssue.AssignedToMember.User.FullName;
                        }
                        
                        // Map Assigned To Team name
                        if (entityIssue.AssignedToTeam != null)
                        {
                            dtoIssue.AssignedTeam = entityIssue.AssignedToTeam.TeamName;
                        }
                    }
                }
            }
            
            return Result.Success(dto);
        }
    }

}

using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Commands;

public record CreateProjectCostCommand(
    Guid ProjectId,              // Required - Project this cost belongs to
    Guid? BoxId,                 // Optional - Specific box (if applicable)
    decimal Cost,
    
    // Cost Code Master fields (cascading)
    string? CostCodeLevel1,      // Required
    string? CostCodeLevel2,      // Required
    string? CostCodeLevel3,      // Optional
    
    // HRC Code fields (cascading)
    string? Chapter,             // Required
    string? SubChapter,          // Required
    string? Classification,      // Required
    string? SubClassification,   // Required
    string? Units,               // Required
    string? Type,                // Required
    
    // Optional references (if user selects existing records)
    Guid? CostCodeId,
    Guid? HRCostRecordId
) : IRequest<Result<ProjectCostDto>>;




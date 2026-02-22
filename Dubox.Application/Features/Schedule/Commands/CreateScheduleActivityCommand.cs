using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

public record CreateScheduleActivityCommand(
    Guid? SourceActivityMasterId,
    bool IsCustomActivity,
    string ActivityCode,
    string ActivityName,
    string Stage,
    int StageNumber,
    int SequenceInStage,
    int OverallSequence,
    string? Description,
    int EstimatedDurationDays,
    bool IsWIRCheckpoint,
    string? WIRCode,
    string? ApplicableBoxTypes,
    string? DependsOnActivities,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    Guid? ProjectId
) : IRequest<Result<Guid>>;
















using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Queries;

public class GetScheduleActivitiesByProjectQueryHandler
    : IRequestHandler<GetScheduleActivitiesByProjectQuery, Result<List<ScheduleActivityListDto>>>
{
    private readonly IDbQueryExecutor _db;

    public GetScheduleActivitiesByProjectQueryHandler(IDbQueryExecutor db)
    {
        _db = db;
    }

    // Replaces:
    //   _context.ScheduleActivities
    //     .Include(AssignedTeams)      ← full JOIN loading all team rows
    //     .Include(AssignedMaterials)  ← full JOIN loading all material rows
    //
    // The view pre-aggregates TeamCount / MaterialCount inside SQL Server.
    // Only the 17 columns the DTO actually needs travel over the wire.
    private const string Sql = @"
        SELECT
            ScheduleActivityId,
            ParentActivityId,
            OverallSequence,
            ActivityCode,
            ActivityName,
            Stage,
            StageNumber,
            IsCustomActivity,
            PlannedStartDate,
            PlannedFinishDate,
            ActualStartDate,
            ActualFinishDate,
            Status,
            PercentComplete,
            Weight,
            TeamCount,
            MaterialCount
        FROM [dbo].[vw_ScheduleActivitiesWithCounts]
        WHERE ProjectId = @projectId
        ORDER BY OverallSequence;";

    public async Task<Result<List<ScheduleActivityListDto>>> Handle(
        GetScheduleActivitiesByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var rows = await _db.QueryListAsync(
            Sql,
            reader => new ScheduleActivityFlatRow(
                ScheduleActivityId: reader.GetGuid(0),
                ParentActivityId:   reader.IsDBNull(1)  ? null : reader.GetGuid(1),
                OverallSequence:    reader.GetInt32(2),
                ActivityCode:       reader.GetString(3),
                ActivityName:       reader.GetString(4),
                Stage:              reader.GetString(5),
                StageNumber:        reader.GetInt32(6),
                IsCustomActivity:   reader.GetBoolean(7),
                PlannedStartDate:   reader.GetDateTime(8),
                PlannedFinishDate:  reader.GetDateTime(9),
                ActualStartDate:    reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                ActualFinishDate:   reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                Status:             reader.GetString(12),
                PercentComplete:    reader.GetDecimal(13),
                Weight:             reader.GetDecimal(14),
                TeamCount:          reader.GetInt32(15),
                MaterialCount:      reader.GetInt32(16)
            ),
            parameters: [("@projectId", request.ProjectId)],
            cancellationToken: cancellationToken);

        if (rows.Count == 0)
            return Result.Success(new List<ScheduleActivityListDto>());

        var hierarchy = ScheduleActivityHierarchyBuilder.BuildHierarchy(rows);
        return Result.Success(hierarchy);
    }
}

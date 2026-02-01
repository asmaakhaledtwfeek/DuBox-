
using Dubox.Application.DTOs;
using Dubox.Application.Features.AuditLogs.Queries;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxLogsQueryHandler : IRequestHandler<GetBoxLogsQuery, Result<PaginatedBoxLogsResponseDto>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public GetBoxLogsQueryHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedBoxLogsResponseDto>> Handle(GetBoxLogsQuery request, CancellationToken cancellationToken)
    {
        // Get box audit logs
        var boxAuditQuery = new GetAuditLogsQuery
        {
            TableName = "Box",
            RecordId = request.BoxId, // BoxId is Guid
            PageNumber = 1, // Get all, we'll handle pagination after combining
            PageSize = 1000, // Large enough to get all box logs
            SearchTerm = request.SearchTerm,
            Action = request.Action,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ChangedBy = request.ChangedBy
        };

        var boxAuditResult = await _mediator.Send(boxAuditQuery, cancellationToken);
        if (!boxAuditResult.IsSuccess)
            return Result.Failure<PaginatedBoxLogsResponseDto>(boxAuditResult.Message);

        var allLogs = new List<AuditLogDto>(boxAuditResult.Data.Items);

        // Get quality issue IDs for this box
        var qualityIssues = _unitOfWork.Repository<QualityIssue>()
            .Get()
            .Where(qi => qi.BoxId == request.BoxId)
            .ToList();
        
        var qualityIssueIds = qualityIssues.Select(qi => qi.IssueId).ToList();

        Console.WriteLine($"📊 Box {request.BoxId} has {qualityIssueIds.Count} quality issues");

        // Get quality issue audit logs
        if (qualityIssueIds.Any())
        {
            var qualityIssueAuditQuery = new GetAuditLogsQuery
            {
                TableName = "QualityIssue",
                PageNumber = 1,
                PageSize = 1000,
                SearchTerm = request.SearchTerm,
                Action = request.Action,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                ChangedBy = request.ChangedBy
            };

            var qualityIssueAuditResult = await _mediator.Send(qualityIssueAuditQuery, cancellationToken);
            if (qualityIssueAuditResult.IsSuccess)
            {
                Console.WriteLine($"📊 Found {qualityIssueAuditResult.Data.Items.Count} total quality issue audit logs");
                
                // Filter to only include logs for quality issues belonging to this box
                // RecordId in AuditLogDto is Guid, so compare directly
                var relevantQualityIssueLogs = qualityIssueAuditResult.Data.Items
                    .Where(log => qualityIssueIds.Contains(log.RecordId))
                    .ToList();
                
                Console.WriteLine($"📊 Filtered to {relevantQualityIssueLogs.Count} relevant quality issue logs for this box");
                
                allLogs.AddRange(relevantQualityIssueLogs);
            }
            else
            {
                Console.WriteLine($"❌ Failed to fetch quality issue audit logs: {qualityIssueAuditResult.Message}");
            }
        }

        // Sort all logs by timestamp descending
        var sortedLogs = allLogs
            .OrderByDescending(log => log.Timestamp)
            .ToList();
        
        Console.WriteLine($"📊 Total combined logs: {sortedLogs.Count} (Box logs + Quality Issue logs)");

        // Apply pagination
        var totalCount = sortedLogs.Count;
        var paginatedLogs = sortedLogs
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Transform AuditLogDto → BoxLogDto
        var boxLogs = paginatedLogs.Select(log => new BoxLogDto
        {
            Id = log.AuditLogId,
            BoxId = request.BoxId, // Already a Guid
            Action = log.Action,
            Description = log.Description,
            TableName = log.TableName, // Include table name for frontend filtering
            EntityDisplayName = log.EntityDisplayName, // Include entity display name
            Field = log.Changes.FirstOrDefault()?.Field,
            OldValue = log.Changes.FirstOrDefault()?.OldValue,
            NewValue = log.Changes.FirstOrDefault()?.NewValue,
            OldValues = log.OldValues, // Include full old values JSON
            NewValues = log.NewValues, // Include full new values JSON
            PerformedByName = log.ChangedByFullName ?? "System",
            PerformedById=log.ChangedBy,
            PerformedAt = log.Timestamp
        }).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var response = new PaginatedBoxLogsResponseDto
        {
            Items = boxLogs,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };

        return Result.Success(response);
    }
}



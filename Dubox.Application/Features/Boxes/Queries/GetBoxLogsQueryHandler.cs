
using Dubox.Application.DTOs;
using Dubox.Application.Features.AuditLogs.Queries;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxLogsQueryHandler : IRequestHandler<GetBoxLogsQuery, Result<PaginatedBoxLogsResponseDto>>
{
    private readonly IMediator _mediator;

    public GetBoxLogsQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<PaginatedBoxLogsResponseDto>> Handle(GetBoxLogsQuery request, CancellationToken cancellationToken)
    {
        var auditQuery = new GetAuditLogsQuery
        {
            TableName = "Box",
            RecordId = request.BoxId,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SearchTerm = request.SearchTerm,
            Action = request.Action,
            FromDate = request.FromDate,
            ToDate = request.ToDate
        };

        var auditResult = await _mediator.Send(auditQuery, cancellationToken);

        if (!auditResult.IsSuccess)
            return Result.Failure<PaginatedBoxLogsResponseDto>(auditResult.Message);

        var logs = auditResult.Data.Items;

        // Transform AuditLogDto → BoxLogDto
        var boxLogs = logs.Select(log => new BoxLogDto
        {
            Id = log.AuditLogId,
            BoxId = request.BoxId,
            Action = log.Action,
            Description = log.Description,
            Field = log.Changes.FirstOrDefault()?.Field,
            OldValue = log.Changes.FirstOrDefault()?.OldValue,
            NewValue = log.Changes.FirstOrDefault()?.NewValue,
            OldValues = log.OldValues, // Include full old values JSON
            NewValues = log.NewValues, // Include full new values JSON
            PerformedBy = log.ChangedByFullName ?? "System",
            PerformedAt = log.Timestamp
        }).ToList();

        var response = new PaginatedBoxLogsResponseDto
        {
            Items = boxLogs,
            TotalCount = auditResult.Data.TotalCount,
            PageNumber = auditResult.Data.PageNumber,
            PageSize = auditResult.Data.PageSize,
            TotalPages = auditResult.Data.TotalPages
        };

        return Result.Success(response);
    }
}



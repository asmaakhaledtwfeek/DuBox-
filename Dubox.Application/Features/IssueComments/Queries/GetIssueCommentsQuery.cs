using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Queries
{
    /// <summary>
    /// Query to get all comments for a quality issue (with threaded structure)
    /// </summary>
    public record GetIssueCommentsQuery(
        Guid IssueId,
        bool IncludeDeleted = false
    ) : IRequest<Result>;
}


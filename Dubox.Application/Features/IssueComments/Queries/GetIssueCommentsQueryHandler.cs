using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.IssueComments.Queries
{
    public class GetIssueCommentsQueryHandler : IRequestHandler<GetIssueCommentsQuery, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetIssueCommentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(GetIssueCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Verify the issue exists
                var issueExists = await _unitOfWork.Repository<QualityIssue>()
                    .GetByIdAsync(request.IssueId, cancellationToken);

                if (issueExists == null)
                {
                    return Result.Failure("Quality issue not found");
                }

                // Get all comments for the issue using specification
                var spec = new GetIssueCommentsSpecification(request.IssueId, request.IncludeDeleted);
                var (commentsQuery, count) = _unitOfWork.Repository<IssueComment>().GetWithSpec(spec);
                var allComments = commentsQuery.ToList();

                // Build threaded structure
                var commentDtos = allComments.Select(c => new IssueCommentDto
                {
                    CommentId = c.CommentId,
                    IssueId = c.IssueId,
                    ParentCommentId = c.ParentCommentId,
                    AuthorId = c.AuthorId,
                    AuthorName = c.Author?.FullName ?? "Unknown",
                    CommentText = c.CommentText,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedByName = c.UpdatedByUser?.FullName,
                    IsDeleted = c.IsDeleted,
                    IsStatusUpdateComment = c.IsStatusUpdateComment,
                    RelatedStatus = c.RelatedStatus,
                    Replies = new List<IssueCommentDto>()
                }).ToList();

                // Build tree structure (top-level comments with nested replies)
                var topLevelComments = new List<IssueCommentDto>();
                var commentMap = commentDtos.ToDictionary(c => c.CommentId);

                foreach (var comment in commentDtos)
                {
                    if (comment.ParentCommentId.HasValue && commentMap.ContainsKey(comment.ParentCommentId.Value))
                    {
                        // This is a reply, add it to parent's replies
                        var parent = commentMap[comment.ParentCommentId.Value];
                        parent.Replies.Add(comment);
                    }
                    else
                    {
                        // This is a top-level comment
                        topLevelComments.Add(comment);
                    }
                }

                // Sort replies by creation date
                foreach (var comment in commentDtos.Where(c => c.Replies.Any()))
                {
                    comment.Replies = comment.Replies.OrderBy(r => r.CreatedDate).ToList();
                }

                return Result.Success(topLevelComments);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving comments: {ex.Message}");
            }
        }
    }
}


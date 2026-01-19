using Dubox.Application.DTOs;
using Dubox.Application.Features.IssueComments.Commands;
using Dubox.Application.Features.IssueComments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers
{
    /// <summary>
    /// Controller for managing issue comments with threading support
    /// </summary>
    [Route("api/issues/{issueId}/comments")]
    [ApiController]
    [Authorize]
    public class IssueCommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssueCommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all comments for a quality issue (with threaded structure)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetIssueComments(
            Guid issueId,
            [FromQuery] bool includeDeleted = false,
            CancellationToken cancellationToken = default)
        {
            var query = new GetIssueCommentsQuery(issueId, includeDeleted);
            var result = await _mediator.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Add a new comment to a quality issue
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddComment(
            Guid issueId,
            [FromBody] CreateCommentRequest request,
            CancellationToken cancellationToken = default)
        {
            if (issueId != request.IssueId)
            {
                return BadRequest("Issue ID mismatch");
            }

            var command = new AddCommentCommand(
                request.IssueId,
                request.ParentCommentId,
                request.CommentText,
                request.IsStatusUpdateComment,
                request.RelatedStatus
            );

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update an existing comment (author only)
        /// </summary>
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(
            Guid issueId,
            Guid commentId,
            [FromBody] UpdateCommentRequest request,
            CancellationToken cancellationToken = default)
        {
            if (commentId != request.CommentId)
            {
                return BadRequest("Comment ID mismatch");
            }

            var command = new UpdateCommentCommand(
                request.CommentId,
                request.CommentText
            );

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Delete a comment (soft delete, author only)
        /// </summary>
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(
            Guid issueId,
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteCommentCommand(commentId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}


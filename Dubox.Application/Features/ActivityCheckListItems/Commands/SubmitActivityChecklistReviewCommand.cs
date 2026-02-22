using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

/// <summary>
/// Command to submit reviews for all checklist items of an activity
/// </summary>
public record SubmitActivityChecklistReviewCommand(
    Guid BoxActivityId,
    List<ReviewActivityCheckListItemDto> Reviews
) : IRequest<Result<string>>;






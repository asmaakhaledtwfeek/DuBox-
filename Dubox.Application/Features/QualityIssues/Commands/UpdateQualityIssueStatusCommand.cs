using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public record UpdateQualityIssueStatusCommand(
        Guid IssueId,
        QualityIssueStatusEnum Status,
        string? ResolutionDescription,
        List<IFormFile>? Files = null,
        List<string>? ImageUrls = null,
        List<string>? FileNames = null
        ) : IRequest<Result<QualityIssueDetailsDto>>;

}

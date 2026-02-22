using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dubox.Application.Services
{
    public interface IQualityIssueMappingService
    {
        QualityIssueDetailsDto MapToDto(QualityIssue issue, bool includeImages = false, IBlobStorageService blobStorageService = null);
        List<QualityIssueDetailsDto> MapToDtoList(IEnumerable<QualityIssue> issues, bool includeImages = false, IBlobStorageService blobStorageService = null);
    }

    public class QualityIssueMappingService : IQualityIssueMappingService
    {
        private const string ImageContainerName = "images";
        public QualityIssueDetailsDto MapToDto(QualityIssue issue, bool includeImages = false, IBlobStorageService blobStorageService = null)
        {
            if (issue == null)
                return null;

            var dto = issue.Adapt<QualityIssueDetailsDto>();

            dto.AssignedToUserName = !string.IsNullOrEmpty(issue.AssignedToMember?.EmployeeName)
                 ? issue.AssignedToMember.EmployeeName
                    : (issue.AssignedToMember?.User?.FullName
                 ?? issue.AssignedUser?.FullName
                 ?? string.Empty);

            // Priority: actual User.UserId > TeamMemberId (for members without user accounts) > direct AssignedUserId.
            // This ensures the dropdown always receives a GUID so backend filtering works
            // even when a TeamMember has no linked user account (UserId is null).
            dto.AssignedToUserId = issue.AssignedToMember?.UserId
                                   ?? issue.AssignedToMemberId
                                   ?? issue.AssignedUserId;

            // Handle Box properties (may be null for project-level issues)
            dto.BoxId = issue.BoxId;
            dto.BoxName = issue.Box?.BoxName ?? string.Empty;
            dto.BoxTag = issue.Box?.BoxTag ?? string.Empty;
            
            // Handle Project properties (get from Box.Project or direct Project)
            if (issue.Project != null)
            {
                dto.ProjectId = issue.Project.ProjectId;
                dto.ProjectName = issue.Project.ProjectName;
                dto.ProjectCode = issue.Project.ProjectCode;
            }
            else if (issue.Box?.Project != null)
            {
                dto.ProjectId = issue.Box.Project.ProjectId;
                dto.ProjectName = issue.Box.Project.ProjectName;
                dto.ProjectCode = issue.Box.Project.ProjectCode;
            }
            else
            {
                dto.ProjectId = issue.ProjectId;
                dto.ProjectName = string.Empty;
                dto.ProjectCode = string.Empty;
            }
            
            // Handle WIR properties
            dto.WIRNumber = issue.WIRCheckpoint?.WIRCode ?? string.Empty;
            dto.WIRName = issue.WIRCheckpoint?.WIRName ?? string.Empty;
            dto.WIRStatus = issue.WIRCheckpoint?.Status;
            dto.WIRRequestedDate = issue.WIRCheckpoint?.RequestedDate;
            dto.InspectorName = issue.WIRCheckpoint?.InspectorName ?? string.Empty;
            
            // Handle Team properties
            dto.AssignedTeamName = issue.AssignedToTeam?.TeamName ?? string.Empty;
            
            // Handle Images if requested
            if (includeImages && blobStorageService != null && issue.Images != null)
            {
                dto.Images = issue.Images
                    .OrderBy(img => img.Sequence)
                    .Select(img => new QualityIssueImageDto
                    {
                        QualityIssueImageId = img.QualityIssueImageId,
                        IssueId = img.IssueId,
                        ImageFileName = img.ImageFileName,
                        ImageUrl = !string.IsNullOrEmpty(img.ImageFileName)
                            ? blobStorageService.GetImageUrl(ImageContainerName, img.ImageFileName)
                            : null,
                        ImageType = img.ImageType,
                        OriginalName = img.OriginalName,
                        FileSize = img.FileSize,
                        Sequence = img.Sequence,
                        Version = img.Version,
                        CreatedDate = img.CreatedDate
                    }).ToList();
            }
            
            return dto;
        }

        public List<QualityIssueDetailsDto> MapToDtoList(IEnumerable<QualityIssue> issues, bool includeImages = false, IBlobStorageService blobStorageService = null)
        {
            if (issues == null)
                return new List<QualityIssueDetailsDto>();

            return issues.Select(issue => MapToDto(issue, includeImages, blobStorageService))
                        .Where(dto => dto != null)
                        .ToList();
        }
    }
}

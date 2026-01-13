using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;
using System.Text.Json;

namespace Dubox.Application.Features.AuditLogs.Queries
{
    public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Result<PaginatedAuditLogsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuditLogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedAuditLogsResponseDto>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
        {
            // Validate pagination parameters
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 25 : (request.PageSize > 100 ? 100 : request.PageSize);

            DateTime? normalizedFrom = null;
            DateTime? normalizedTo = null;

            if (request.FromDate.HasValue)
            {
                var from = request.FromDate.Value;
                normalizedFrom = DateTime.SpecifyKind(from, from.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : from.Kind)
                    .Date;
            }

            if (request.ToDate.HasValue)
            {
                var to = request.ToDate.Value;
                var normalized = DateTime.SpecifyKind(to, to.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : to.Kind)
                    .Date
                    .AddDays(1)
                    .AddTicks(-1);
                normalizedTo = normalized;
            }

            var searchParams = new AuditLogSearchParams
            {
                TableName = request.TableName,
                Action = request.Action,
                RecordId = request.RecordId,
                SearchTerm = request.SearchTerm,
                FromDate = normalizedFrom,
                ToDate = normalizedTo,
                ChangedBy = request.ChangedBy,
            };

            // Create specification with pagination
            var specification = new AuditLogSearchSpecification(searchParams, pageSize, pageNumber);

            var logs = _unitOfWork.Repository<AuditLog>()
                .GetWithSpec(specification);

            var logEntities = logs.Data.ToList();
            var totalCount = logs.Count;

            var logDtos = new List<AuditLogDto>();

            foreach (var log in logEntities)
            {
                var dto = _mapper.Map<AuditLogDto>(log);

                // Map AuditId to AuditLogId if mapper didn't handle it
                if (dto.AuditLogId == Guid.Empty)
                {
                    dto.AuditLogId = log.AuditId;
                }

                // Map Timestamp from ChangedDate
                dto.Timestamp = log.ChangedDate;

                // Resolve entity display name
                dto.EntityDisplayName = await ResolveEntityDisplayNameAsync(log.TableName, log.RecordId, cancellationToken);

                // Resolve user full name
                if (log.ChangedBy.HasValue)
                {
                    dto.ChangedByFullName = await ResolveUserNameAsync(log.ChangedBy.Value, cancellationToken);
                }

                // Generate diff from oldValues and newValues
                dto.Changes = GenerateFieldChanges(log.OldValues, log.NewValues);

                logDtos.Add(dto);
            }

            // Note: SearchTerm filtering is done in-memory after pagination
            // For better performance with large datasets, consider moving SearchTerm to database query
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLowerInvariant();
                logDtos = logDtos.Where(dto =>
                        (!string.IsNullOrWhiteSpace(dto.EntityDisplayName) && dto.EntityDisplayName!.ToLowerInvariant().Contains(term)) ||
                        (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description.ToLowerInvariant().Contains(term)) ||
                        (!string.IsNullOrWhiteSpace(dto.TableName) && dto.TableName.ToLowerInvariant().Contains(term)) ||
                        dto.Changes.Any(change =>
                            (!string.IsNullOrWhiteSpace(change.Field) && change.Field.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrWhiteSpace(change.OldValue) && change.OldValue!.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrWhiteSpace(change.NewValue) && change.NewValue!.ToLowerInvariant().Contains(term))
                        ))
                    .ToList();
            }

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedAuditLogsResponseDto
            {
                Items = logDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Result.Success(response);
        }

        private async Task<string?> ResolveEntityDisplayNameAsync(string? tableName, Guid? recordId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(tableName) || !recordId.HasValue)
                return null;

            try
            {
                return tableName.ToLower() switch
                {
                    "project" or "projects" => await ResolveProjectNameAsync(recordId.Value, cancellationToken),
                    "box" or "boxes" => await ResolveBoxNameAsync(recordId.Value, cancellationToken),
                    "boxactivity" or "boxactivities" => await ResolveBoxActivityNameAsync(recordId.Value, cancellationToken),
                    "qualityissue" or "qualityissues" => await ResolveQualityIssueNameAsync(recordId.Value, cancellationToken),
                    "user" or "users" => await ResolveUserNameAsync(recordId.Value, cancellationToken),
                    "team" or "teams" => await ResolveTeamNameAsync(recordId.Value, cancellationToken),
                    "material" or "materials" => await ResolveMaterialNameAsync(recordId.Value, cancellationToken),
                    _ => null
                };
            }
            catch
            {
                return null;
            }
        }

        private async Task<string?> ResolveProjectNameAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);
            return project != null ? $"{project.ProjectName} ({project.ProjectCode})" : null;
        }

        private async Task<string?> ResolveBoxNameAsync(Guid boxId, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
            return box != null ? $"{box.BoxName ?? box.BoxTag} ({box.BoxTag})" : null;
        }

        private async Task<string?> ResolveBoxActivityNameAsync(Guid activityId, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(activityId, cancellationToken);
            return activity != null ? $"Activity #{activity.Sequence}" : null;
        }

        private async Task<string?> ResolveQualityIssueNameAsync(Guid issueId, CancellationToken cancellationToken)
        {
            var issue = await _unitOfWork.Repository<QualityIssue>().GetByIdAsync(issueId, cancellationToken);
            return issue != null ? $"Issue: {issue.IssueType}" : null;
        }

        private async Task<string?> ResolveUserNameAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken);
            return user != null ? (user.FullName ?? user.Email) : null;
        }

        private async Task<string?> ResolveTeamNameAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>().GetByIdAsync(teamId, cancellationToken);
            return team != null ? team.TeamName : null;
        }

        private async Task<string?> ResolveMaterialNameAsync(Guid materialId, CancellationToken cancellationToken)
        {
            var material = await _unitOfWork.Repository<Material>().GetByIdAsync(materialId, cancellationToken);
            return material != null ? material.MaterialName : null;
        }

        private List<FieldChangeDto> GenerateFieldChanges(string? oldValuesJson, string? newValuesJson)
        {
            var changes = new List<FieldChangeDto>();

            if (string.IsNullOrWhiteSpace(oldValuesJson) && string.IsNullOrWhiteSpace(newValuesJson))
                return changes;

            Dictionary<string, object?>? oldValues = null;
            Dictionary<string, object?>? newValues = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(oldValuesJson))
                {
                    oldValues = JsonSerializer.Deserialize<Dictionary<string, object?>>(oldValuesJson);
                }
            }
            catch { }

            try
            {
                if (!string.IsNullOrWhiteSpace(newValuesJson))
                {
                    newValues = JsonSerializer.Deserialize<Dictionary<string, object?>>(newValuesJson);
                }
            }
            catch { }

            if (oldValues == null && newValues == null)
                return changes;

            var allKeys = new HashSet<string>();
            if (oldValues != null)
                foreach (var key in oldValues.Keys) allKeys.Add(key);
            if (newValues != null)
                foreach (var key in newValues.Keys) allKeys.Add(key);

            foreach (var key in allKeys)
            {
                var oldVal = oldValues?.ContainsKey(key) == true ? FormatValue(oldValues[key]) : null;
                var newVal = newValues?.ContainsKey(key) == true ? FormatValue(newValues[key]) : null;

                // Skip if values are the same
                if (oldVal == newVal)
                    continue;

                // Skip internal/system fields
                if (ShouldSkipField(key))
                    continue;

                changes.Add(new FieldChangeDto
                {
                    Field = FormatFieldName(key),
                    OldValue = oldVal,
                    NewValue = newVal
                });
            }

            return changes;
        }

        private bool ShouldSkipField(string fieldName)
        {
            var skipFields = new[] { "Id", "CreatedDate", "ModifiedDate", "CreatedBy", "ModifiedBy", "AuditId", "ChangedBy", "ChangedDate" };
            return skipFields.Any(f => fieldName.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        private string FormatFieldName(string fieldName)
        {
            // Convert PascalCase to readable format
            return System.Text.RegularExpressions.Regex.Replace(
                fieldName,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                " $1",
                System.Text.RegularExpressions.RegexOptions.Compiled
            ).Trim();
        }

        private string? FormatValue(object? value)
        {
            if (value == null)
                return null;

            if (value is JsonElement jsonElement)
            {
                return jsonElement.ValueKind switch
                {
                    System.Text.Json.JsonValueKind.String => jsonElement.GetString(),
                    System.Text.Json.JsonValueKind.Number => jsonElement.GetDecimal().ToString("N2"),
                    System.Text.Json.JsonValueKind.True => "Yes",
                    System.Text.Json.JsonValueKind.False => "No",
                    System.Text.Json.JsonValueKind.Null => null,
                    _ => value.ToString()
                };
            }

            if (value is DateTime dateTime)
                return dateTime.ToString("MMM dd, yyyy");

            if (value is decimal || value is double || value is float)
                return Convert.ToDecimal(value).ToString("N2");

            if (value is bool boolVal)
                return boolVal ? "Yes" : "No";

            return value.ToString();
        }
    }
}

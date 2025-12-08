using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record UpdateCompressionStartDateCommand(Guid ProjectId, DateTime? CompressionStartDate) : IRequest<Result<ProjectDto>>;


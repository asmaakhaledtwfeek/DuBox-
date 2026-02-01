using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Domain.Services
{
    public interface ITeamAssignmentService
    {
        Task<Result<AssignmentValidationResult>> ValidateAssignmentAsync(Guid? teamId, Guid? memberId,CancellationToken cancellationToken = default);
    }
    public record AssignmentValidationResult(Team? Team, TeamMember? Member);
}

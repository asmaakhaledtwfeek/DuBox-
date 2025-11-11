using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{

    public record ComplateTeamMemberProfileCommand(
    Guid TeamMemberId,
    string EmployeeCode,
    string EmployeeName,
    string? MobileNumber
) : IRequest<Result<TeamMemberDto>>;
}

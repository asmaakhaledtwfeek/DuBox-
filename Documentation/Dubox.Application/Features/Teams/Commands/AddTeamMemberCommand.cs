using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public record AddTeamMemberCommand(
    Guid TeamId,
    string FirstName,
    string LastName,
    string EmployeeCode,
    bool IsCreateAccount,
    string? Email,
    string? TemporaryPassword
) : IRequest<Result<TeamMemberDto>>;


using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class AddTeamMemberCommandHandler : IRequestHandler<AddTeamMemberCommand, Result<TeamMemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IPasswordHasher _passwordHasher;

    public AddTeamMemberCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<TeamMemberDto>> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var canCreate = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
        if (!canCreate)
        {
            return Result.Failure<TeamMemberDto>("Access denied. Only System Administrators and Project Managers can add team members.");
        }

        var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.TeamId, cancellationToken);
        if (team == null)
            return Result.Failure<TeamMemberDto>("Team not found.");

        if (!team.IsActive)
            return Result.Failure<TeamMemberDto>("Cannot add members to an inactive team.");

        var employeeCodeExists = await _unitOfWork.Repository<TeamMember>()
            .IsExistAsync(tm => tm.TeamId == request.TeamId && tm.EmployeeCode == request.EmployeeCode, cancellationToken);

        if (employeeCodeExists)
            return Result.Failure<TeamMemberDto>("This employee code already exists in this team.");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        Guid? userId = null;

        // Create user account if requested
        if (request.IsCreateAccount)
        {
            var existingUser =  _unitOfWork.Repository<User>()
                .FindAsync(u => u.Email == request.Email!, cancellationToken).Result.FirstOrDefault();

            if (existingUser != null)
                    return Result.Failure<TeamMemberDto>("Cannot add this User, this email elready exist for another user .");
            
                var newUser = new User
                {
                    Email = request.Email!,
                    PasswordHash = _passwordHasher.HashPassword(request.TemporaryPassword!),
                    FullName = $"{request.FirstName} {request.LastName}",
                    DepartmentId = team.DepartmentId,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<User>().AddAsync(newUser, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
                userId = newUser.UserId;

                // Create audit log for user creation
                var userAuditLog = new AuditLog
                {
                    TableName = nameof(User),
                    RecordId = newUser.UserId,
                    Action = "Creation",
                    OldValues = "N/A (New Entity)",
                    NewValues = $"Email: {newUser.Email}, FullName: {newUser.FullName}, DepartmentId: {newUser.DepartmentId}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"New User account '{newUser.Email}' created for team member."
                };
                await _unitOfWork.Repository<AuditLog>().AddAsync(userAuditLog, cancellationToken);
            
        }

        // Create team member
        var teamMember = new TeamMember
        {
            TeamId = request.TeamId,
            UserId = userId,
            EmployeeCode = request.EmployeeCode,
            EmployeeName = $"{request.FirstName} {request.LastName}",
            IsActive = true
        };

        await _unitOfWork.Repository<TeamMember>().AddAsync(teamMember, cancellationToken);

        // Create audit log for team member
        var teamMemberAuditLog = new AuditLog
        {
            TableName = nameof(TeamMember),
            RecordId = teamMember.TeamMemberId,
            Action = "Creation",
            OldValues = "N/A (New Entity)",
            NewValues = $"TeamId: {teamMember.TeamId}, EmployeeName: {teamMember.EmployeeName}, EmployeeCode: {teamMember.EmployeeCode}, UserId: {teamMember.UserId?.ToString() ?? "N/A"}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New Team Member '{teamMember.EmployeeName}' (Code: {teamMember.EmployeeCode}) added to team '{team.TeamName}'."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(teamMemberAuditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Retrieve the created team member with navigation properties
        var createdTeamMember =  _unitOfWork.Repository<TeamMember>()
            .FindAsync(tm => tm.TeamMemberId == teamMember.TeamMemberId, cancellationToken).Result.FirstOrDefault();

        // Map to DTO
        var response = new TeamMemberDto
        {
            TeamMemberId = createdTeamMember!.TeamMemberId,
            UserId = createdTeamMember.UserId ?? Guid.Empty,
            TeamId = createdTeamMember.TeamId,
            TeamCode = team.TeamCode,
            TeamName = team.TeamName,
            Email = request.IsCreateAccount ? request.Email! : string.Empty,
            FullName = createdTeamMember.EmployeeName,
            EmployeeCode = createdTeamMember.EmployeeCode ?? string.Empty,
            EmployeeName = createdTeamMember.EmployeeName,
            MobileNumber = createdTeamMember.MobileNumber
        };

        var successMessage = request.IsCreateAccount
            ? $"Team member added successfully with user account created."
            : $"Team member added successfully without user account.";

        return Result.Success(response, successMessage);
    }
}


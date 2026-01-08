using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
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
        var module = PermissionModuleEnum.Teams;
        var action = PermissionActionEnum.ManageMembers;
        var canAddMember = await _visibilityService.CanPerformAsync(module,action,cancellationToken);
        if (!canAddMember)
            return Result.Failure<TeamMemberDto>("Access denied. Only System Administrators and Project Managers can add Crew members.");

        var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.TeamId, cancellationToken);
        if (team == null)
            return Result.Failure<TeamMemberDto>("Crew not found.");

        if (!team.IsActive)
            return Result.Failure<TeamMemberDto>("Cannot add members to an inactive Crew.");

        var employeeCodeExists = await _unitOfWork.Repository<TeamMember>()
            .IsExistAsync(tm => tm.TeamId == request.TeamId && tm.EmployeeCode == request.EmployeeCode, cancellationToken);

        if (employeeCodeExists)
            return Result.Failure<TeamMemberDto>("This employee code already exists in this Crew.");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        Guid? userId = null;

        if(request.IsCreateAccount)
        {
           await CreateUserWithAuditAsync(request,team.DepartmentId,currentUserId,cancellationToken);
        }
        // Create team member
        var response = await CreateTeamMemberWithAuditAsync(request, team, userId, currentUserId, cancellationToken);
        var successMessage = request.IsCreateAccount
            ? $"Crew member added successfully with user account created."
            : $"Crew member added successfully without user account.";

        return Result.Success(response, successMessage);
    }

    private async Task<Result<Guid>> CreateUserWithAuditAsync(AddTeamMemberCommand request,Guid departmentId,Guid currentUserId, CancellationToken cancellationToken)
    {
        var existingUser = (await _unitOfWork.Repository<User>()
            .FindAsync(u => u.Email == request.Email!, cancellationToken))
            .FirstOrDefault();

        if (existingUser != null)
            return Result.Failure<Guid>("Cannot add this User, this email already exists for another user.");

        var newUser = new User
        {
            Email = request.Email!,
            PasswordHash = _passwordHasher.HashPassword(request.TemporaryPassword!),
            FullName = $"{request.FirstName} {request.LastName}",
            DepartmentId = departmentId,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<User>().AddAsync(newUser, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var userAuditLog = new AuditLog
        {
            TableName = nameof(User),
            RecordId = newUser.UserId,
            Action = "Creation",
            OldValues = "N/A (New Entity)",
            NewValues = $"Email: {newUser.Email}, FullName: {newUser.FullName}, DepartmentId: {newUser.DepartmentId}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New User account '{newUser.Email}' created for Crew member."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(userAuditLog, cancellationToken);
        return Result.Success(newUser.UserId);
    }
   
    private async Task<TeamMemberDto> CreateTeamMemberWithAuditAsync(AddTeamMemberCommand request,Team team, Guid? userId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var teamMember = new TeamMember
        {
            TeamId = team.TeamId,
            UserId = userId,
            EmployeeCode = request.EmployeeCode,
            EmployeeName = $"{request.FirstName} {request.LastName}",
            IsActive = true
        };

        await _unitOfWork.Repository<TeamMember>().AddAsync(teamMember, cancellationToken);

        var teamMemberAuditLog = new AuditLog
        {
            TableName = nameof(TeamMember),
            RecordId = teamMember.TeamMemberId,
            Action = "Creation",
            OldValues = "N/A (New Entity)",
            NewValues = $"CrewId: {teamMember.TeamId}, EmployeeName: {teamMember.EmployeeName}, EmployeeCode: {teamMember.EmployeeCode}, UserId: {teamMember.UserId?.ToString() ?? "N/A"}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New Crew Member '{teamMember.EmployeeName}' (Code: {teamMember.EmployeeCode}) added to Crew '{team.TeamName}'."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(teamMemberAuditLog, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        var response = new TeamMemberDto
        {
            TeamMemberId = teamMember!.TeamMemberId,
            UserId = teamMember.UserId ?? Guid.Empty,
            TeamId = teamMember.TeamId,
            TeamCode = team.TeamCode,
            TeamName = team.TeamName,
            Email = request.IsCreateAccount ? request.Email! : string.Empty,
            FullName = teamMember.EmployeeName,
            EmployeeCode = teamMember.EmployeeCode ?? string.Empty,
            EmployeeName = teamMember.EmployeeName,
            MobileNumber = teamMember.MobileNumber
        };
        return response;
    }

}


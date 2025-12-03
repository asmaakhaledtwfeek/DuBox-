using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TeamDto>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var teamExists = await _unitOfWork.Repository<Team>()
            .IsExistAsync(t => t.TeamCode == request.TeamCode, cancellationToken);

        if (teamExists)
            return Result.Failure<TeamDto>("Team with this code already exists");

        var team = request.Adapt<Team>();
        team.IsActive = true;
        team.CreatedDate = DateTime.UtcNow;

        await _unitOfWork.Repository<Team>().AddAsync(team, cancellationToken);
        
        // Create audit log
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var auditLog = new AuditLog
        {
            TableName = nameof(Team),
            RecordId = team.TeamId,
            Action = "Creation",
            OldValues = "N/A (New Entity)",
            NewValues = $"Code: {team.TeamCode}, Name: {team.TeamName}, DepartmentId: {team.DepartmentId}, Trade: {team.Trade ?? "N/A"}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New Team '{team.TeamCode} - {team.TeamName}' created successfully."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
        
        await _unitOfWork.CompleteAsync(cancellationToken);
        var createdTeam = _unitOfWork.Repository<Team>()
              .GetWithSpec(new GetTeamWithIncludesSpecification(team.TeamId)).Data.First();
        var response = _mapper.Map<TeamDto>(createdTeam);
        return Result.Success(response);
    }
}


namespace Dubox.Application.DTOs;

public record TeamDto
{
    public Guid TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public string? Trade { get; init; }
    public string? TeamLeaderName { get; init; }
    public int? TeamSize { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
}

public record CreateTeamDto
{
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Trade { get; init; }
    public string? TeamLeaderName { get; init; }
    public int? TeamSize { get; init; }
}

public record UpdateTeamDto
{
    public Guid TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Trade { get; init; }
    public string? TeamLeaderName { get; init; }
    public int? TeamSize { get; init; }
    public bool IsActive { get; init; }
}

public record TeamProductivityDto
{
    public Guid TeamId { get; init; }
    public string TeamName { get; init; } = string.Empty;
    public int TotalActivitiesAssigned { get; init; }
    public int CompletedActivities { get; init; }
    public int InProgressActivities { get; init; }
    public decimal AverageProgressPercentage { get; init; }
    public int TotalUpdatesToday { get; init; }
    public int TotalBoxesWorkedOn { get; init; }
}
public record TeamMembersDto
{
    public Guid TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public int TeamSize { get; init; } = 0;
    public List<TeamMemberDto> Members { get; init; } = new();

}
public record TeamMemberDto
{
    public Guid TeamMemberId { get; set; }
    public Guid? UserId { get; init; }
    public Guid TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string? Email { get; init; } 
    public string? FullName { get; init; }
    public string? EmployeeCode { get; set; } 
    public string? EmployeeName { get; set; } 
    public string? MobileNumber { get; set; }
    public bool IsActive { get; init; } = true;
}

public record PaginatedTeamsResponseDto : PaginatedResponse<TeamDto>;

public record TeamGroupDto
{
    public Guid TeamGroupId { get; init; }
    public Guid TeamId { get; init; }
    public string TeamName { get; init; } = string.Empty;
    public string TeamCode { get; init; } = string.Empty;
    public string GroupTag { get; init; } = string.Empty;
    public string GroupType { get; init; } = string.Empty;
    public Guid? GroupLeaderId { get; init; }
    public string? GroupLeaderName { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public string? CreatedBy { get; init; } = string.Empty;
    public int MemberCount { get; init; }
    public List<TeamMemberDto> Members { get; init; } = new();
}

public record PaginatedTeamGroupsResponseDto : PaginatedResponse<TeamGroupDto>;

public record TeamGroupMembersDto
{
    public Guid TeamGroupId { get; init; }
    public Guid TeamId { get; init; }
    public string TeamCode { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public string GroupTag { get; init; } = string.Empty;
    public string GroupType { get; init; } = string.Empty;
    public Guid? GroupLeaderId { get; init; }
    public string? GroupLeaderName { get; init; }
    public int MemberCount { get; init; } = 0;
    public bool IsActive { get; init; }
    public List<TeamMemberDto> Members { get; init; } = new();
}




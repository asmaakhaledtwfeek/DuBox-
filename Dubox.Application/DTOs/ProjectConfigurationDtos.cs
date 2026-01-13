namespace Dubox.Application.DTOs;

public record ProjectBuildingDto
{
    public int Id { get; init; }
    public Guid ProjectId { get; init; }
    public string BuildingCode { get; init; } = string.Empty;
    public string? BuildingName { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}

public record ProjectLevelDto
{
    public int Id { get; init; }
    public Guid ProjectId { get; init; }
    public string LevelCode { get; init; } = string.Empty;
    public string? LevelName { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}

public record ProjectBoxTypeDto
{
    public int Id { get; init; }
    public Guid ProjectId { get; init; }
    public string TypeName { get; init; } = string.Empty;
    public string? Abbreviation { get; init; }
    public bool HasSubTypes { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
    public List<ProjectBoxSubTypeDto> SubTypes { get; init; } = new();
}

public record ProjectBoxSubTypeDto
{
    public int Id { get; init; }
    public int ProjectBoxTypeId { get; init; }
    public string SubTypeName { get; init; } = string.Empty;
    public string? Abbreviation { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}

public record ProjectZoneDto
{
    public int Id { get; init; }
    public Guid ProjectId { get; init; }
    public string ZoneCode { get; init; } = string.Empty;
    public string? ZoneName { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}

public record ProjectBoxFunctionDto
{
    public int Id { get; init; }
    public Guid ProjectId { get; init; }
    public string FunctionName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}

public record ProjectConfigurationDto
{
    public Guid ProjectId { get; init; }
    public List<ProjectBuildingDto> Buildings { get; init; } = new();
    public List<ProjectLevelDto> Levels { get; init; } = new();
    public List<ProjectBoxTypeDto> BoxTypes { get; init; } = new();
    public List<ProjectZoneDto> Zones { get; init; } = new();
    public List<ProjectBoxFunctionDto> BoxFunctions { get; init; } = new();
}


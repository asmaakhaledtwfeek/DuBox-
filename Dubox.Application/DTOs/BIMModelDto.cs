namespace Dubox.Application.DTOs;

public record BIMModelDto(
    Guid BIMModelId,
    string ModelName,
    string? Category,
    string? RevitFamily,
    string? Type,
    string? Instance,
    decimal? Quantity,
    string? Unit,
    DateTime? PlannedStartDate,
    DateTime? PlannedFinishDate,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate,
    string? ModelFilePath,
    string? ThumbnailPath,
    Guid? ProjectId,
    string? Description
);

public record BIMModelListDto(
    Guid BIMModelId,
    string ModelName,
    string? Category,
    string? RevitFamily,
    string? Type,
    decimal? Quantity,
    string? Unit
);

public record CreateBIMModelCommand(
    string ModelName,
    string? Category,
    string? RevitFamily,
    string? Type,
    string? Instance,
    decimal? Quantity,
    string? Unit,
    DateTime? PlannedStartDate,
    DateTime? PlannedFinishDate,
    Guid? ProjectId,
    string? Description
);




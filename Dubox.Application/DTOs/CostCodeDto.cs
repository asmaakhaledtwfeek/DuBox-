namespace Dubox.Application.DTOs;

public record CostCodeDto(
    Guid CostCodeId,
    string Code,
    string Description,
    string? Category,
    string? SubCategory,
    string? UnitOfMeasure,
    decimal? UnitRate,
    string Currency,
    string? Notes,
    bool IsActive,
    int DisplayOrder
);

public record CreateCostCodeDto(
    string Code,
    string Description,
    string? Category,
    string? SubCategory,
    string? UnitOfMeasure,
    decimal? UnitRate,
    string Currency,
    string? Notes
);

public record CostCodeListDto(
    Guid CostCodeId,
    string Code,
    string? CostCodeLevel1,
    string? Level1Description,
    string? CostCodeLevel2,
    string? Level2Description,
    string? CostCodeLevel3,
    string Description,
    string? Level3DescriptionAbbrev,
    string? Level3DescriptionAmana,
    string? Category,
    string? UnitOfMeasure,
    decimal? UnitRate,
    string Currency,
    bool IsActive
);




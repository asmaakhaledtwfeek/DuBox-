using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Factories.Commands;

public record CreateFactoryCommand(
    string FactoryCode,
    string FactoryName,
    ProjectLocationEnum Location,
    int? Capacity,
    int MinRow,
    int MaxRow,
    string MinBay,
    string MaxBay
) : IRequest<Result<FactoryDto>>;


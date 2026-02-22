using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;
using System;

namespace Dubox.Application.Features.Projects.Queries
{
    public record GetProjectWeatherReportQuery(Guid ProjectId) : IRequest<Result<ProjectWeatherReportDto>>;
}

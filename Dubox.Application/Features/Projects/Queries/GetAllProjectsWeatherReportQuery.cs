using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;
using System.Collections.Generic;

namespace Dubox.Application.Features.Projects.Queries
{
    public record GetAllProjectsWeatherReportQuery : IRequest<Result<List<ProjectWeatherReportDto>>>;
}

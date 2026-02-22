using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dubox.Application.Features.Projects.Queries
{
    public class GetAllProjectsWeatherReportQueryHandler : IRequestHandler<GetAllProjectsWeatherReportQuery, Result<List<ProjectWeatherReportDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProjectsWeatherReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ProjectWeatherReportDto>>> Handle(GetAllProjectsWeatherReportQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // Get all active projects with actual start date
            var activeProjects = await _unitOfWork.Repository<Project>()
                .FindAsync(p => p.IsActive && 
                               p.Status == ProjectStatusEnum.Active &&
                               p.ActualStartDate.HasValue && 
                               p.ActualStartDate.Value.Date <= today, 
                          cancellationToken);

            var projectList = activeProjects.ToList();

            // Get all weather reports for today
            var weatherReports = await _unitOfWork.Repository<WeatherDataCache>()
                .FindAsync(r => r.LastUpdated.Date == today, cancellationToken);

            var weatherReportsList = weatherReports.ToList();

            // Join projects with weather reports
            //var result = projectList
            //    .GroupJoin(
            //        weatherReportsList,
            //        p => p.ProjectId,
            //        p=>p.pr
            //        (project, reports) => new { Project = project, Report = reports.FirstOrDefault() }
            //    )
            //    .Where(x => x.Report != null) // Only include projects with weather reports
            //    .Select(x => new ProjectWeatherReportDto
            //    {
            //        ReportId = x.Report!.ReportId,
            //        ProjectId = x.Project.ProjectId,
            //        ProjectCode = x.Project.ProjectCode,
            //        ProjectName = x.Project.ProjectName,
            //        ReportDate = x.Report.ReportDate,
            //        CurrentTemperature = x.Report.CurrentTemperature,
            //        MinTemperature = x.Report.MinTemperature,
            //        MaxTemperature = x.Report.MaxTemperature,
            //        Humidity = x.Report.Humidity,
            //        PrecipitationProbability = x.Report.PrecipitationProbability,
            //        PrecipitationAmount = x.Report.PrecipitationAmount,
            //        WindSpeed = x.Report.WindSpeed,
            //        WindGust = x.Report.WindGust,
            //        WindDirection = x.Report.WindDirection,
            //        Pressure = x.Report.Pressure,
            //        SolarRadiation = x.Report.SolarRadiation,
            //        Sunrise = x.Report.Sunrise,
            //        Sunset = x.Report.Sunset,
            //        Moonrise = x.Report.Moonrise,
            //        Moonset = x.Report.Moonset,
            //        Latitude = x.Report.Latitude,
            //        Longitude = x.Report.Longitude,
            //        Elevation = x.Report.Elevation,
            //        Description = x.Report.Description,
            //        IsFavorable = x.Report.IsFavorable,
            //        AlertMessage = x.Report.AlertMessage,
            //        QualityIssueCreated = x.Report.QualityIssueCreated,
            //        CreatedDate = x.Report.CreatedDate
            //    })
            //    .OrderBy(x => x.ProjectCode)
            //    .ToList();

            return Result.Success(new List<ProjectWeatherReportDto>());
        }
    }
}

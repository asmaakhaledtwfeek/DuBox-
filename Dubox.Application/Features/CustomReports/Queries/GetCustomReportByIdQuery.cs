using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

public record GetCustomReportByIdQuery(Guid Id) : IRequest<Result<CustomReportDto>>;

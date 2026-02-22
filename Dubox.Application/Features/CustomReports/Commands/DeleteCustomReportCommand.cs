using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Commands;

public record DeleteCustomReportCommand(Guid Id) : IRequest<Result>;

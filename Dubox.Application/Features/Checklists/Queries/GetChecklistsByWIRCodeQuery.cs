using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Queries;

public record GetChecklistsByWIRCodeQuery(string WIRCode) : IRequest<Result<List<ChecklistDto>>>;

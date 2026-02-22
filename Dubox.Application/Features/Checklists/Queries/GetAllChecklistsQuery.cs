using Dubox.Application.DTOs;
using Dubox.Application.Features.Checklists.Queries;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Queries;

public record GetAllChecklistsQuery : IRequest<Result<List<ChecklistDto>>>;

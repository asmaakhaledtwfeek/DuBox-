using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Groups.Queries;

public record GetAllGroupsQuery : IRequest<Result<List<GroupDto>>>;


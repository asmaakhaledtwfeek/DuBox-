using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypes.Queries;

public record GetAllProjectTypeCategoriesQuery : IRequest<Result<List<ProjectTypeCategoryDto>>>;


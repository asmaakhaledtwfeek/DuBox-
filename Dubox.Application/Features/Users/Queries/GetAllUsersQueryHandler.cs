using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<PaginatedUsersResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedUsersResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // Validate pagination parameters
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 25 : (request.PageSize > 100 ? 100 : request.PageSize);

        // Create specification with pagination
        var specification = new GetUserWithIcludesSpecification(pageSize, pageNumber);

        var usersResult = _unitOfWork.Repository<User>().GetWithSpec(specification);
        var users = usersResult.Data.ToList();
        var totalCount = usersResult.Count;

        var userDtos = users.Adapt<List<UserDto>>();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var response = new PaginatedUsersResponseDto
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return Result.Success(response);
    }
}


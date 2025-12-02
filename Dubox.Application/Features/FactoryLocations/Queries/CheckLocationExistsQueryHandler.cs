using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Queries;

public class CheckLocationExistsQueryHandler : IRequestHandler<CheckLocationExistsQuery, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckLocationExistsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(CheckLocationExistsQuery request, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.Repository<Domain.Entities.FactoryLocation>()
            .IsExistAsync(l => l.LocationCode == request.LocationCode, cancellationToken);

        return Result.Success(exists);
    }
}


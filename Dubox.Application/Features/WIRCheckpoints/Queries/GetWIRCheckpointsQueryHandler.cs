using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointsQueryHandler : IRequestHandler<GetWIRCheckpointsQuery, Result<List<WIRCheckpointDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWIRCheckpointsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<WIRCheckpointDto>>> Handle(GetWIRCheckpointsQuery request, CancellationToken cancellationToken)
        {
            var checkPoints = _unitOfWork.Repository<WIRCheckpoint>()
                 .GetWithSpec(new GetWIRCheckpointsSpecification(request));

            return Result.Success(checkPoints.Adapt<List<WIRCheckpointDto>>());
        }
    }

}

using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointByIdQueryHandler
    : IRequestHandler<GetWIRCheckpointByIdQuery, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWIRCheckpointByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(GetWIRCheckpointByIdQuery request, CancellationToken cancellationToken)
        {

            var checkpoint = _unitOfWork.Repository<WIRCheckpoint>().
                GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (checkpoint == null)
                return Result.Failure<WIRCheckpointDto>("WIR checkpoint not found");

            return Result.Success(checkpoint.Adapt<WIRCheckpointDto>());
        }
    }

}

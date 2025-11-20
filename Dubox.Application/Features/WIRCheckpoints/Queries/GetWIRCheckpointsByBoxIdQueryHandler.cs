using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointsByBoxIdQueryHandler
     : IRequestHandler<GetWIRCheckpointsByBoxIdQuery, Result<List<WIRCheckpointDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWIRCheckpointsByBoxIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<WIRCheckpointDto>>> Handle(GetWIRCheckpointsByBoxIdQuery request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (box == null)
                return Result.Failure<List<WIRCheckpointDto>>("Box not found");

            var checkPOints = _unitOfWork.Repository<WIRCheckpoint>().
                GetWithSpec(new GetWIRCheckPointsByBoxIdSpecification(request.BoxId)).Data.ToList();

            return Result.Success(checkPOints.Adapt<List<WIRCheckpointDto>>());

        }
    }

}

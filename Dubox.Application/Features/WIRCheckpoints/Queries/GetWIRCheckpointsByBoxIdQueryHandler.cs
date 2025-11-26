using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using System.Linq;

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

            var checkpoints = _unitOfWork.Repository<WIRCheckpoint>()
                .GetWithSpec(new GetWIRCheckPointsByBoxIdSpecification(request.BoxId)).Data.ToList();

            var checkpointDtos = checkpoints.Adapt<List<WIRCheckpointDto>>();

            await PopulateActivityMetadata(checkpointDtos, cancellationToken);

            return Result.Success(checkpointDtos);
        }

        private async Task PopulateActivityMetadata(List<WIRCheckpointDto> checkpoints, CancellationToken cancellationToken)
        {
            if (checkpoints.Count == 0)
            {
                return;
            }

            var wirCodes = checkpoints
                .Select(cp => cp.WIRNumber)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!wirCodes.Any())
            {
                return;
            }

            var wirRecords = _unitOfWork.Repository<WIRRecord>()
                .GetWithSpec(new GetWIRRecordsByCodesSpecification(wirCodes)).Data.ToList();

            var recordMap = wirRecords
                .GroupBy(record => record.WIRCode.ToLower())
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .OrderByDescending(r => r.CreatedDate)
                        .First());

            foreach (var checkpoint in checkpoints)
            {
                var key = checkpoint.WIRNumber?.ToLower();
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                if (!recordMap.TryGetValue(key, out var record))
                {
                    continue;
                }

                checkpoint.BoxActivityId = record.BoxActivityId;
                checkpoint.ProjectId ??= record.BoxActivity?.Box?.ProjectId;
            }
        }
    }

}

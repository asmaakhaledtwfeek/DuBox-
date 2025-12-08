using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            // Use AsNoTracking and ToListAsync for better performance
            var checkPoints = await _unitOfWork.Repository<WIRCheckpoint>()
                 .GetWithSpec(new GetWIRCheckpointsSpecification(request)).Data
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);

            var checkpointDtos = checkPoints.Adapt<List<WIRCheckpointDto>>();

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

            // Use AsNoTracking and ToListAsync for better performance
            var wirRecords = await _unitOfWork.Repository<WIRRecord>()
                .GetWithSpec(new GetWIRRecordsByCodesSpecification(wirCodes)).Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

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

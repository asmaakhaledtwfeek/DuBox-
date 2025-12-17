using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries
{
    public class GetBoxDrawingQueryHandler : IRequestHandler<GetBoxDrawingQuery, Result<List<ProgressUpdateImageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBoxDrawingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<ProgressUpdateImageDto>>> Handle(GetBoxDrawingQuery request, CancellationToken cancellationToken)
        {
            var boxIsExist = await _unitOfWork.Repository<Box>().IsExistAsync(x => x.BoxId == request.boxId);
            if (!boxIsExist)
                return Result.Failure<List<ProgressUpdateImageDto>>("Box not found");
            var progressUpdateIds = _unitOfWork.Repository<ProgressUpdate>().Get().Where(pu => pu.BoxId == request.boxId).
                Select(pu => pu.ProgressUpdateId);
            var progressImages = _unitOfWork.Repository<ProgressUpdateImage>().Get()
                .Where(img => progressUpdateIds.Contains(img.ProgressUpdateId)).ToList();
            var dto = progressImages.Adapt<List<ProgressUpdateImageDto>>();
            return Result.Success(dto);
        }
    }
}

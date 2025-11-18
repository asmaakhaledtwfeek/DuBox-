using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.AuditLogs
{
    public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Result<List<AuditLogDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuditLogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<AuditLogDto>>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var searchParams = new AuditLogSearchParams
            {
                TableName = request.TableName,
                RecordId = request.RecordId,
                SearchTerm = request.SearchTerm,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
            };

            var logs = _unitOfWork.Repository<AuditLog>()
                .GetWithSpec(new AuditLogSearchSpecification(searchParams));

            var logDtos = _mapper.Map<List<AuditLogDto>>(logs);

            return Result.Success(logDtos);
        }
    }
}

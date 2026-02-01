using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using MapsterMapper;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateHRCostCommandHandler : IRequestHandler<CreateHRCostCommand, Result<CreateHRCostResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateHRCostCommandHandler> _logger;
    private readonly IMapper _mapper;
    public CreateHRCostCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<CreateHRCostCommandHandler> logger
        ,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<CreateHRCostResponse>> Handle(CreateHRCostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if code already exists (if code is provided)
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var codeExists = await _unitOfWork.Repository<HRCostRecord>()
                    .IsExistAsync(c => c.Code == request.Code, cancellationToken);

                if (codeExists)
                    return Result.Failure<CreateHRCostResponse>(new Error("DuplicateCode", $"HR cost code '{request.Code}' already exists."));
            }
            var hrCost = _mapper.Map<HRCostRecord>(request);

            hrCost.CreatedBy = _currentUserService.UserId ?? "System";
            hrCost.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<HRCostRecord>().AddAsync(hrCost, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Create audit log for HR cost creation
            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var userId) ? userId : Guid.Empty;
            var auditLog = new AuditLog
            {
                TableName = nameof(HRCostRecord),
                RecordId = hrCost.HRCostRecordId,
                Action = "INSERT",
                OldValues = null,
                NewValues = $"Code: {hrCost.Code ?? "N/A"}, Chapter: {hrCost.Chapter ?? "N/A"}, Name: {hrCost.Name}, Units: {hrCost.Units ?? "N/A"}, Type: {hrCost.Type ?? "N/A"}, Status: {hrCost.Status ?? "N/A"}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"HR cost '{hrCost.Name}' created successfully."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("HR cost {Name} created successfully by user {UserId}", 
                request.Name, _currentUserService.UserId);

            var response = new CreateHRCostResponse(
                hrCost.HRCostRecordId,
                hrCost.Code,
                hrCost.Name
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating HR cost {Name}", request.Name);
            return Result.Failure<CreateHRCostResponse>(new Error("CreateError", $"Error creating HR cost: {ex.Message}"));
        }
    }
}


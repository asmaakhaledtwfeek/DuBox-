using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MapsterMapper;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateCostCodeCommandHandler : IRequestHandler<CreateCostCodeCommand, Result<CreateCostCodeResponse>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateCostCodeCommandHandler> _logger;
    private readonly IMapper _mapper;
    public CreateCostCodeCommandHandler(
        IDbContext context,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<CreateCostCodeCommandHandler> logger, IMapper mapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<CreateCostCodeResponse>> Handle(CreateCostCodeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if cost code already exists
            var codeExists = await _unitOfWork.Repository<CostCodeMaster>()
                .IsExistAsync(c => c.Code == request.Code, cancellationToken);

            if (codeExists)
                return Result.Failure<CreateCostCodeResponse>(new Error("DuplicateCode", $"Cost code '{request.Code}' already exists."));
            var costCode= _mapper.Map<CostCodeMaster>(request);
            costCode.CreatedDate=DateTime.Now;
            costCode.CreatedBy=_currentUserService.UserId??"System";

            await _unitOfWork.Repository<CostCodeMaster>().AddAsync(costCode, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Create audit log for cost code creation
            var currentUserId = Guid.TryParse(_currentUserService.UserId, out var userId) ? userId : Guid.Empty;
            var auditLog = new AuditLog
            {
                TableName = nameof(CostCodeMaster),
                RecordId = costCode.CostCodeId,
                Action = "INSERT",
                OldValues = null,
                NewValues = $"Code: {costCode.Code}, Description: {costCode.Description}, Level1: {costCode.CostCodeLevel1 ?? "N/A"}, Level2: {costCode.CostCodeLevel2 ?? "N/A"}, Level3: {costCode.CostCodeLevel3 ?? "N/A"}, Category: {costCode.Category ?? "N/A"}, Unit: {costCode.UnitOfMeasure ?? "N/A"}, Rate: {costCode.UnitRate?.ToString() ?? "N/A"}, Currency: {costCode.Currency}, Status: {(costCode.IsActive ? "Active" : "Inactive")}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Cost code '{costCode.Code}' created successfully."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Cost code {Code} created successfully by user {UserId}", 
                request.Code, _currentUserService.UserId);

            var response = new CreateCostCodeResponse(
                costCode.CostCodeId,
                costCode.Code,
                costCode.Description
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating cost code {Code}", request.Code);
            return Result.Failure<CreateCostCodeResponse>(new Error("CreateError", $"Error creating cost code: {ex.Message}"));
        }
    }
}


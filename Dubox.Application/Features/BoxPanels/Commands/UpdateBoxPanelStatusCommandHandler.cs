using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxPanels.Commands;

public class UpdateBoxPanelStatusCommandHandler : IRequestHandler<UpdateBoxPanelStatusCommand, Result<BoxPanelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public UpdateBoxPanelStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<BoxPanelDto>> Handle(UpdateBoxPanelStatusCommand request, CancellationToken cancellationToken)
    {
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
            .FirstOrDefaultAsync(p => p.BoxPanelId == request.BoxPanelId, cancellationToken);

        if (panel == null)
            return Result.Failure<BoxPanelDto>("Panel not found");

        // Check if box is dispatched - cannot modify panels
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<BoxPanelDto>("Cannot update panel status. Box is dispatched and read-only.");

        panel.PanelStatus = request.PanelStatus;
        panel.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.Repository<BoxPanel>().Update(panel);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = new BoxPanelDto
        {
            BoxPanelId = panel.BoxPanelId,
            BoxId = panel.BoxId,
            ProjectId = panel.ProjectId,
            PanelName = panel.PanelName,
            PanelStatus = panel.PanelStatus,
            CreatedDate = panel.CreatedDate,
            ModifiedDate = panel.ModifiedDate
        };

        return Result.Success(dto);
    }
}

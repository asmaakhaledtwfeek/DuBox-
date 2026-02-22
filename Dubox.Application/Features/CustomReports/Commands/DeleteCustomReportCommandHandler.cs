using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Commands;

public class DeleteCustomReportCommandHandler : IRequestHandler<DeleteCustomReportCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomReportCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCustomReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _unitOfWork.Repository<CustomReport>()
                .GetByIdAsync(request.Id, cancellationToken);

            if (report is null)
                return Result.Failure($"Custom report {request.Id} not found.");

            _unitOfWork.Repository<CustomReport>().Delete(report);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Report deleted.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete report: {ex.Message}");
        }
    }
}

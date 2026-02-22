using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries
{
    public class GetBoxExchangeHistoryQueryHandler
        : IRequestHandler<GetBoxExchangeHistoryQuery, Result<List<BoxExchangeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetBoxExchangeHistoryQueryHandler(
            IUnitOfWork unitOfWork,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
        }

        public async Task<Result<List<BoxExchangeDto>>> Handle(
            GetBoxExchangeHistoryQuery request,
            CancellationToken cancellationToken)
        {
            // Get box to check project access
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (box == null)
                return Result.Failure<List<BoxExchangeDto>>("Box not found.");

            // Verify user has access to the project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<List<BoxExchangeDto>>("Access denied. You do not have permission to view this box.");

            // Get all box exchanges with related data using specification
            var specification = new GetBoxExchangesByBoxIdSpecification(request.BoxId);
            var exchanges = _unitOfWork.Repository<BoxExchange>().GetWithSpec(specification).Data.ToList();

            // Map to DTOs
            var dtos = new List<BoxExchangeDto>();
            foreach (var exchange in exchanges)
            {
                var dto = exchange.Adapt<BoxExchangeDto>();
                dto.BoxTag = exchange.Box?.BoxTag;
                dto.BoxName = exchange.Box?.BoxName;
                dto.IssueNumber = exchange.QualityIssue?.IssueNumber;
                dto.RequestedByName = exchange.RequestedByUser?.FullName;
                dto.ApprovedByName = exchange.ApprovedByUser?.FullName;
                dto.RejectedByName = exchange.RejectedByUser?.FullName;
                dto.AppliedByName = exchange.AppliedByUser?.FullName;
                
                // Add exchanged with box information
                dto.ExchangedWithBoxId = exchange.ExchangedWithBoxId;
                dto.ExchangedWithBoxProjectId = exchange.ExchangedWithBox?.ProjectId;
                dto.ExchangedWithBoxTag = exchange.ExchangedWithBox?.BoxTag;
                dto.ExchangedWithBoxName = exchange.ExchangedWithBox?.BoxName;
                
                dtos.Add(dto);
            }

            return Result.Success(dtos);
        }
    }
}

using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Services
{
    public class BoxActivityService : IBoxActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;

        public BoxActivityService(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task CopyActivitiesToBox(Box box, CancellationToken cancellationToken)
        {
            // Get BoxType name from ProjectBoxTypes (since BoxTypeId references project configuration)
            string? boxType = null;
            if (box.ProjectBoxTypeId.HasValue)
            {
                var projectBoxType = await _dbContext.ProjectBoxTypes
                    .Where(pbt => pbt.Id == box.ProjectBoxTypeId.Value && pbt.ProjectId == box.ProjectId)
                    .FirstOrDefaultAsync(cancellationToken);
                boxType = projectBoxType?.TypeName?.Trim();
            }
            
            var searchPattern = $",{boxType},";

            var activityMasters = await _dbContext.ActivityMasters
                .Where(am => am.IsActive &&
                    (string.IsNullOrEmpty(am.ApplicableBoxTypes) ||
                     am.ApplicableBoxTypes.Contains(searchPattern)))
                .OrderBy(am => am.OverallSequence)
                .ToListAsync(cancellationToken);

            var boxActivities = activityMasters.Select(am => new BoxActivity
            {
                BoxId = box.BoxId,
                ActivityMasterId = am.ActivityMasterId,
                Sequence = am.OverallSequence,
                Status = BoxStatusEnum.NotStarted,
                ProgressPercentage = 0,
                MaterialsAvailable = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }).ToList();

            await _unitOfWork.Repository<BoxActivity>().AddRangeAsync(boxActivities, cancellationToken);
        }

        public async Task ResetBoxActivitiesFromTemplateAsync(Box box, Guid activityTemplateId, CancellationToken cancellationToken)
        {
            if (box.Status != BoxStatusEnum.NotStarted && box.Status != BoxStatusEnum.ReadyToStart)
                return;

            var existingActivities = await _dbContext.BoxActivities
                .Where(ba => ba.BoxId == box.BoxId)
                .ToListAsync(cancellationToken);

            if (existingActivities.Any())
            {
                _dbContext.BoxActivities.RemoveRange(existingActivities);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            await CopyActivitiesFromTemplateToBox(box, activityTemplateId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task CopyActivitiesFromTemplateToBox(Box box, Guid activityTemplateId, CancellationToken cancellationToken)
        {
            // Get the activity template with its activities
            var template = await _dbContext.ActivityTemplates
                .Include(t => t.TemplateActivities)
                .FirstOrDefaultAsync(t => t.ActivityTemplateId == activityTemplateId && t.IsActive, cancellationToken);

            if (template == null)
            {
                throw new InvalidOperationException($"Activity template with ID {activityTemplateId} not found or is not active.");
            }

            // Get the highest existing sequence number for this box to avoid duplicates
            var maxExistingSequence = await _dbContext.BoxActivities
                .Where(ba => ba.BoxId == box.BoxId)
                .MaxAsync(ba => (int?)ba.Sequence, cancellationToken) ?? 0;

            // Create BoxActivities from template activities with unique sequential numbers
            var boxActivities = new List<BoxActivity>();
            int currentSequence = maxExistingSequence + 1;
            
            foreach (var templateActivity in template.TemplateActivities.OrderBy(a => a.OverallSequence))
            {
                var boxActivity = new BoxActivity
                {
                    BoxId = box.BoxId,
                    // All template activities are custom (no link to ActivityMaster)
                    ActivityMasterId = null,
                    // Link to the template activity itself for traceability
                    ActivityTemplateActivityId = templateActivity.ActivityTemplateActivityId,
                    // Assign unique sequence number
                    Sequence = currentSequence++,
                    Status = BoxStatusEnum.NotStarted,
                    ProgressPercentage = 0,
                    MaterialsAvailable = true,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                boxActivities.Add(boxActivity);
            }

            if (boxActivities.Any())
            {
                await _unitOfWork.Repository<BoxActivity>().AddRangeAsync(boxActivities, cancellationToken);
            }
        }
    }

}

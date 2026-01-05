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
    }

}

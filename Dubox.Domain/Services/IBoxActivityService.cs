using Dubox.Domain.Entities;

namespace Dubox.Domain.Services
{
    public interface IBoxActivityService
    {
        Task CopyActivitiesToBox(Box box, CancellationToken cancellationToken);
        Task CopyActivitiesFromTemplateToBox(Box box, Guid activityTemplateId, CancellationToken cancellationToken);

        /// <summary>
        /// Resets a box's activities by deleting existing ones and recreating them from the
        /// specified template.  Only acts on boxes whose status is NotStarted or ReadyToStart;
        /// all other statuses are left untouched.
        /// </summary>
        Task ResetBoxActivitiesFromTemplateAsync(Box box, Guid activityTemplateId, CancellationToken cancellationToken);
    }
}

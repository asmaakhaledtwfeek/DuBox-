using Dubox.Domain.Entities;

namespace Dubox.Domain.Services
{
    public interface IBoxActivityService
    {
        Task CopyActivitiesToBox(Box box, CancellationToken cancellationToken);
    }
}

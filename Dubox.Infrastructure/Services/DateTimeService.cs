using Dubox.Infrastructure.Abstraction;

namespace Dubox.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}

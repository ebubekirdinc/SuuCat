using Notification.Application.Common.Interfaces;

namespace Notification.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

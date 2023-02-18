using Subscription.Application.Common.Interfaces;

namespace Subscription.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

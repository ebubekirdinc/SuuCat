using Payment.Application.Common.Interfaces;

namespace Payment.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

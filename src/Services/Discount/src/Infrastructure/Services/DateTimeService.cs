using Discount.Application.Common.Interfaces;

namespace Discount.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

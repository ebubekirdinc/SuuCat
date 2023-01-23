using Assessment.Application.Common.Interfaces;

namespace Assessment.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

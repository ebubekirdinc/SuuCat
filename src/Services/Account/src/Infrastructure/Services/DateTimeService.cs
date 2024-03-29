﻿using Account.Application.Common.Interfaces;

namespace Account.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

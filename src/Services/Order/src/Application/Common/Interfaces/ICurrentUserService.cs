﻿namespace Order.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}

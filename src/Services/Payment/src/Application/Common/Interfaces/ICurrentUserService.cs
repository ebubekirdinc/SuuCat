﻿namespace Payment.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}

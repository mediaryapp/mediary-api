﻿namespace Infrastructure.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}

using System;
using SportsCenter.Core.Abstractions;

namespace SportsCenter.Infrastructure.Time;

internal sealed class Clock : IClock
{
    public DateTime Current()
    {
        return DateTime.UtcNow;
    }
}
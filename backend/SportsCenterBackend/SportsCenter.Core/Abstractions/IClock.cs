using System;

namespace SportsCenter.Core.Abstractions;

public interface IClock
{
    DateTime Current();
}
using System;

namespace SportsCenter.Core.Exceptions;

public class CustomException : Exception
{
    protected CustomException(string? message) : base(message)
    {
    }
}
using System.Collections.Generic;
using FluentValidation.Results;
using SportsCenter.Core.Exceptions;

namespace SportsCenter.Application.Exceptions;

public sealed class ValidationException : CustomException
{
    public List<ValidationFailure> ValidationErrors { get; set; }

    public ValidationException(List<ValidationFailure> validationErrors) : base("Error during command/query validation")
    {
        ValidationErrors = validationErrors;
    }
}
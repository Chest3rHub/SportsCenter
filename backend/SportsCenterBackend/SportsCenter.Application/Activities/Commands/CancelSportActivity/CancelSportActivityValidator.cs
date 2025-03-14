using FluentValidation;
using System;

namespace SportsCenter.Application.Activities.Commands.CancelSportActivity
{
    public class CancelSportActivityValidator : AbstractValidator<CancelSportActivity>
    {
        public CancelSportActivityValidator()
        {
            RuleFor(x => x.SportActivityId)
                .GreaterThan(0).WithMessage("SportActivityId must be greater than zero.");

            RuleFor(x => x.ActivityDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("ActivityDate must be in the future.");
        }
    }
}

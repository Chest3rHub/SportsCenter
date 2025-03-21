using FluentValidation;
using System;

namespace SportsCenter.Application.Activities.Commands.SignUpForActivity
{
    public class SignUpForActivityValidator : AbstractValidator<SignUpForActivity>
    {
        public SignUpForActivityValidator()
        {
            RuleFor(x => x.ActivityId)
                .GreaterThan(0)
                .WithMessage("ActivityId must be greater than 0.");

            RuleFor(x => x.SelectedDate)
                .NotNull()
                .WithMessage("SelectedDate is required.")
                .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("SelectedDate must be today or a future date.");

            RuleFor(x => x.IsEquipmentIncluded)
                .NotNull()
                .WithMessage("IsEquipmentIncluded is required.")
                .Must(x => x == true || x == false)
                .WithMessage("IsEquipmentIncluded must be true or false.");

        }
    }
}

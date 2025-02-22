using FluentValidation;
using System;

namespace SportsCenter.Application.SportsClubManagement.Commands.AddSportsClubWorkingHours
{
    public class SetSportsCenterWorkingHoursValidator : AbstractValidator<SetSportsCenterWorkingHours>
    {
        public SetSportsCenterWorkingHoursValidator()
        {
            RuleFor(x => x.OpenHour)
                .LessThan(x => x.CloseHour)
                .WithMessage("Open hour must be earlier than close hour.");
        }
    }
}

using FluentValidation;
using SportsCenter.Application.SportsClubManagement.Commands.AddSportsClubWorkingHours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Commands.SetSpecialSportsCenterWorkingHours
{
    public class SetSpecialSportsCenterWorkingHoursValidator : AbstractValidator<SetSpecialSportsCenterWorkingHours>
    {
        public SetSpecialSportsCenterWorkingHoursValidator()
        {
            RuleFor(x => x.OpenHour)
                .NotEmpty().WithMessage("Open hour is required")
                .LessThan(x => x.CloseHour)
                .WithMessage("Open hour must be earlier than close hour");

            RuleFor(x => x.CloseHour)
                .NotEmpty().WithMessage("Close hour is required");

            RuleFor(x => x.Date)
               .NotEmpty().WithMessage("Date is required")
               .GreaterThanOrEqualTo(DateTime.Today).WithMessage("the date cannot be from the past");
        }
    }
}
